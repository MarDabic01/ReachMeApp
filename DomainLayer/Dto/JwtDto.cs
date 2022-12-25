using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class JwtDto
    {
        public string UserIdentifier { get; set; }
        public string GivenName { get; set; }
        public string Email { get; set; }
    }
}
