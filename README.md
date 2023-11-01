# AudioChatGpt
Simple react app, for audio requests to OpenAI

This is a React app that allows you to send audio requests to OpenAI. 
![mainAudio](https://github.com/zedicus25/AudioChatGpt/assets/95874337/d45fbdbb-a7ec-408f-917c-1a356742dfad)

On the Front-End part, audio is recorded, up to 15 seconds, then a request is sent to AWS rekognition services using Asp .Net, which converts the audio to text and the text request is sent to the OpenAI API.

Also, there are user roles that limit the number of requests and, depending on the role, the user can send only text requests or text and pictures or audio requests. The roles are implemented using ASP.NET Core Identity.

Request page:
![request](https://github.com/zedicus25/AudioChatGpt/assets/95874337/bbf86c13-36bd-4869-8240-f18e6dfbaa71)

History page:
![history](https://github.com/zedicus25/AudioChatGpt/assets/95874337/06f2f9e8-0233-41e5-b498-249b2fe8fb8f)
