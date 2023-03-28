using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using static System.Net.Mime.MediaTypeNames;
using Amazon.Rekognition.Model;
using Amazon.Rekognition;

namespace AudioGhatGPT.Controllers
{
    public class TranscribeHelper
    {
        private readonly  RegionEndpoint _region;
        private readonly AmazonTranscribeServiceClient _transcribeClient;
        private readonly AmazonS3Client _s3Client;
        private readonly AmazonRekognitionClient _rekognitionClient;

        private string _bucketName;

        public TranscribeHelper()
        {
            _region = RegionEndpoint.EUWest2;
            _bucketName = ConfigurationManager.AppSettings["Amazon:BusketName"];
            _s3Client = new AmazonS3Client(ConfigurationManager.AppSettings["Amazon:AccessKey"],
                ConfigurationManager.AppSettings["Amazon:SecretKey"], _region);
            _transcribeClient = new AmazonTranscribeServiceClient(ConfigurationManager.AppSettings["Amazon:AccessKey"],
                ConfigurationManager.AppSettings["Amazon:SecretKey"], _region);
            _rekognitionClient = new AmazonRekognitionClient(ConfigurationManager.AppSettings["Amazon:AccessKey"],
                ConfigurationManager.AppSettings["Amazon:SecretKey"], _region);
        }


        public async Task<string> TranscribeMediaFile(IFormFile file)
        {
            var mediaFileName = file.FileName;

            var s3HttpUri = await UploadFileToBucket(file);

            var pos = mediaFileName.LastIndexOf(".");
            var transcriptFileName = (pos != -1) ? mediaFileName.Substring(0, pos) + ".json" : mediaFileName + ".json";

            var startJobRequest = new StartTranscriptionJobRequest()
            {
                Media = new Media()
                {
                    MediaFileUri = s3HttpUri
                },
                OutputBucketName = _bucketName,
                OutputKey = transcriptFileName,
                IdentifyMultipleLanguages = true,
                LanguageOptions = new List<string> {
                    "en-US",
                    "ru-RU"
                },
                TranscriptionJobName = $"{Guid.NewGuid()}-{mediaFileName}"
            };
            var startJobResponse = await _transcribeClient.StartTranscriptionJobAsync(startJobRequest);

            var getJobRequest = new GetTranscriptionJobRequest() 
            { TranscriptionJobName = startJobRequest.TranscriptionJobName };
            GetTranscriptionJobResponse getJobResponse;
            do
            {
                Thread.Sleep(1000);
                getJobResponse = await _transcribeClient.GetTranscriptionJobAsync(getJobRequest);
            } while (getJobResponse.TranscriptionJob.TranscriptionJobStatus == "IN_PROGRESS");

            await SaveS3ObjectAsFile(_bucketName, transcriptFileName, transcriptFileName);
            var data = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(transcriptFileName));
            var results = data["results"]["transcripts"][0]["transcript"].Value<string>();
            File.Delete(transcriptFileName);

            await DeleteObjectFromBucket(transcriptFileName, _bucketName);
            return results;
        }

        private async Task<string> UploadFileToBucket(IFormFile file, bool returnUri = true)
        {
            string fileKey = Guid.NewGuid().ToString() +"_"+ file.FileName;
            using (var newMemoryStream = new MemoryStream())
            {
                file.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileKey,
                    BucketName = _bucketName,
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            var httpUri = $"https://{_bucketName}.s3.amazonaws.com/{fileKey}";
            if(returnUri)
                return httpUri;
            return fileKey;
        }

        private async Task SaveS3ObjectAsFile(string bucketName, string key, string filePath)
        {
            using (var obj = await _s3Client.GetObjectAsync(bucketName, key))
            {
                await obj.WriteResponseStreamToFileAsync(filePath, false, new CancellationToken());
            }
        }


        public async Task<string> GetLinesFromImage(IFormFile file)
        {
            string fileInfo = "";
            string fileName = await UploadFileToBucket(file, false);

            DetectTextRequest detectTextRequest = new DetectTextRequest()
            {
                Image = new Amazon.Rekognition.Model.Image()
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object()
                    {
                        Name = fileName,
                        Bucket = _bucketName,
                        
                    }
                }
            };

            try
            {
                DetectTextResponse detectTextResponse = _rekognitionClient.DetectTextAsync(detectTextRequest).GetAwaiter().GetResult();
                foreach (TextDetection text in detectTextResponse.TextDetections.Where(x => x.Type == TextTypes.LINE))
                {
                    fileInfo += text.DetectedText + " \n";
                }
            }
            catch (Exception e)
            {
            }
            return fileInfo;
        }

        private async Task DeleteObjectFromBucket(string filename, string bucketName)
        {
            Console.WriteLine($"Delete {filename} from bucket {bucketName}");
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = Path.GetFileName(filename)
            };
            await _s3Client.DeleteObjectAsync(deleteRequest);
        }
    }
}
