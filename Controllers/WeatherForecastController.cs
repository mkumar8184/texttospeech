using Microsoft.AspNetCore.Mvc;
using System.Speech.Synthesis;

namespace texttospeechnetcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SpeechSynthesizer _speechSynthesizer;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _speechSynthesizer = new SpeechSynthesizer();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
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




    }
}
