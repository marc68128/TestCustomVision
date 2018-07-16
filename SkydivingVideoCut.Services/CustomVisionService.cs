using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkidivingVideoCut.Domain.Dto;

namespace SkydivingVideoCut.Services
{
    public class CustomVisionService
    {
        public async Task<CustomVisionResult> Analyse(string imagePath)
        {
            var bytes = File.ReadAllBytes(imagePath);
            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, @"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/15bec530-eb81-4445-924f-7647adbde630/image?iterationId=8faf2109-a8ec-4490-a191-2163cb139110");
                requestMessage.Headers.Add("Prediction-Key", "254bf7d900494664b420fac171b69a66");
                requestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                requestMessage.Content = new ByteArrayContent(bytes);

                var resp = await client.SendAsync(requestMessage);
                if (resp.IsSuccessStatusCode)
                {
                    var respString = await resp.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CustomVisionResult>(respString);
                }
            }

            return null; 
        }
    }
}
