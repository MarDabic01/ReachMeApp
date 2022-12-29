using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class PostDto
    {
        public int UserId { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
