using DomainLayer.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace ReachMeApp.Components
{
    public class DisplayPostsBase : ComponentBase
    {
        [Parameter]
        public int MyId { get; set; }
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public List<Post> Posts { get; set; }
    }
}
