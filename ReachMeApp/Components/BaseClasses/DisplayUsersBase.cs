using DomainLayer.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace ReachMeApp.Components
{
    public class DisplayUsersBase : ComponentBase
    {
        [Parameter]
        public List<User> Users { get; set; }
    }
}
