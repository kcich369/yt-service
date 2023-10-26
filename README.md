# yt-service

The application is an advanced tool that analyzes video content. By leveraging cloud solutions such as Azure and powerful AI models, primarily chatGPT, users gain access to the exploration of video content, primarily on the YouTube platform. This is achieved by simply adding the channel name through the API interface. Subsequently, all audio-format videos belonging to the specific channel are downloaded, allowing users to listen to their content. Whenever a new video is posted on the channel, it is automatically downloaded. Each time, the application ensures the language of the audio file and generates a transcription based on it, which can be translated into any language. This enables users to access individual summaries of each video in their preferred language.

Furthermore, a list of tags is created, enabling the identification of topics discussed in the videos. The application also facilitates quick and effective searching of specific content within multimedia. Users input text, which is analyzed and verified, and the application then returns specific moments in the videos related to the query. This means that users can easily locate the portions of interest in multiple videos, significantly streamlining the content analysis process.

Additionally, the enhanced version of this application allows for comprehensive analysis of YouTube channels with diverse themes.

# business logic

The process starts from controller: ytchannels, action 'Create'.
The brain of whole app is located in Infastructure.Services. 
