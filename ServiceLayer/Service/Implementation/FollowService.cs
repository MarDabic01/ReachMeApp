using DomainLayer.Model;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Service.Implementation
{
    public class FollowService : IFollow
    {
        private readonly DataContext context;
        private readonly IUser userService;

        public FollowService(DataContext context, IUser userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public void FollowUser(int followerId, int followingId)
        {
            Follow newFollow = new Follow
            {
                FollowerId = followerId,
                FollowingId = followingId
            };
            context.Follows.Add(newFollow);
            context.SaveChanges();
        }

        public void UnfollowUser(int followerId, int followingId)
        {
            List<Follow> allFollowers = context.Follows.Where(f => f.FollowingId == followingId).ToList();
            foreach (Follow f in allFollowers)
                if (f.FollowerId == followerId)
                    context.Follows.Remove(f);
            context.SaveChanges();
        }

        public bool IsAlreadyFollowing(int followerId, int followingId)
        {
            List<Follow> allFollowers = context.Follows.Where(f => f.FollowingId == followingId).ToList();
            foreach (Follow f in allFollowers)
                if (f.FollowerId == followerId)
                    return true;
            return false;
        }

        public List<User> GetFollowers(User user)
        {
            List<int> followerIds = context.Follows.Where(f => f.FollowingId == user.Id).Select(u => u.FollowerId).ToList();
            return context.Users.Where(u => followerIds.Contains(u.Id)).ToList();
        }

        public List<User> GetFollowings(User user)
        {
            List<int> followingsIds = context.Follows.Where(f => f.FollowerId == user.Id).Select(u => u.FollowingId).ToList();
            return context.Users.Where(u => followingsIds.Contains(u.Id)).ToList();
        }

        public List<User> GetSuggestions(User user)
        {
            List<int> usersThatIFollow = context.Follows.Where(f => f.FollowerId == user.Id).Select(u => u.FollowingId).ToList();
            List<int> usersThatMyFollowersFollow = context.Follows.Where(f => usersThatIFollow.Contains(f.FollowerId)).Select(f => f.FollowingId).ToList();
            List<int> suggestedUsersId = usersThatMyFollowersFollow.Except(usersThatIFollow).ToList();
            if (suggestedUsersId.Contains(user.Id))
                suggestedUsersId.Remove(user.Id);

            return context.Users.Where(u => suggestedUsersId.Contains(u.Id)).ToList();
        }
    }
}
