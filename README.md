# text to speech in .Net 6.0
# Free text to speech in .net core
# Install nuget package :system.speech depends on .net version
 Even though its used in window app but you can use in your .net core app as well.
copy paste below code in your web api controller

 [HttpGet("textspeech")]
 public async Task<IActionResult> GetTextToSpeech(string text)
 {
     byte[] audioBytes = await TextToSpeechConverter(text);

     if (audioBytes != null)
     {

       //  var base64Content = Convert.ToBase64String(audioBytes); if you want you can change in base64 and return string
         
        return File(audioBytes, "audio/webm", "output_audio.mp3");
     }
     else
     {
         return BadRequest("Failed to generate audio.");
     }

 }
 private async Task<byte[]> TextToSpeechConverter(string text) {


     _speechSynthesizer.SetOutputToDefaultAudioDevice();
     //synth.Rate = -1;
     _speechSynthesizer.Volume = 100;
     _speechSynthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Senior);
     MemoryStream stream = new MemoryStream();
     _speechSynthesizer.SetOutputToWaveStream(stream);
     _speechSynthesizer.Speak(text);
      byte[] speechBytes = stream.GetBuffer();
     return speechBytes;

 }
