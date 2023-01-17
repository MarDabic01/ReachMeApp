using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DomainLayer.Dto
{
    public class PostDto
    {
        public int UserId { get; set; }
        public string Description { get; set; }
        public string ImageData { get; set; }
        [JsonIgnore]
        public IFormFile ImageFile { get; set; }
    }
}
