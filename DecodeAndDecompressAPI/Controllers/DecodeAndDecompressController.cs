using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace DecodeAndDecompressAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DecodeAndDecompressController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post([FromBody] DataRequest? request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            try
            {
                // Decode the data using the provided key
                var decodedData = Decode(request.Data!, request.Key!);

                // Decompress the decoded data
                var decompressedData = Decompress(decodedData);
                
                // Deserialize the decompressed data into a JSON object
                var jsonObject = JsonSerializer.Deserialize<JsonObject>(decompressedData);

                // Send the decompressed data as the response
                return Ok(jsonObject);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private string Decode(string base64Data, string key)
        {
            return DecryptAndDecompress.Decrypt(base64Data, key);
        }

        private string Decompress(string data)
        {
            return DecryptAndDecompress.Decompress(data);
        }
    }

    public class DataRequest
    {
        public string? Data { get; set; }
        public string? Key { get; set; }
    }
}
