using Microsoft.AspNetCore.Components;

namespace ReachMeApp.Components
{
    public class FollowButtonBase : ComponentBase
    {
        [Parameter]
        public int FollowerId { get; set; }
        [Parameter]
        public int FollowingId { get; set; }
    }
}
