# AudioChatGpt
Simple react app, for audio requests to OpenAI

This is a React app that allows you to send audio requests to OpenAI. 

On the Front-End part, audio is recorded, up to 15 seconds, then a request is sent to AWS rekognition services using Asp .Net, which converts the audio to text and the text request is sent to the OpenAI API.

Also, there are user roles that limit the number of requests and, depending on the role, the user can send only text requests or text and pictures or audio requests. The roles are implemented using ASP.NET Core Identity.
