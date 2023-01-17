using DomainLayer.Model;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Service.Implementation
{
    public class FollowService : IFollow
    {
        private readonly DataContext context;

        public FollowService(DataContext context)
        {
            this.context = context;
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

        public List<User> GetFollowers(string username)
        {
            List<User> followers = new List<User>();
            User currentUser = context.Users.FirstOrDefault(u => u.Username == username);
            List<Follow> followsIds = context.Follows.Where(f => f.FollowingId == currentUser.Id).ToList();
            foreach (Follow f in followsIds)
                followers.Add(context.Users.FirstOrDefault(u => u.Id == f.FollowerId));
            return followers;
        }

        public List<User> GetFollowings(string username)
        {
            List<User> followings = new List<User>();
            User currentUser = context.Users.FirstOrDefault(u => u.Username == username);
            List<Follow> followingsIds = context.Follows.Where(f => f.FollowerId == currentUser.Id).ToList();
            foreach (Follow f in followingsIds)
                followings.Add(context.Users.FirstOrDefault(u => u.Id == f.FollowingId));
            return followings;
        }

        public List<User> GetSuggestions(string username)
        {
            User currentUser = context.Users.FirstOrDefault(u => u.Username == username);
            List<Follow> myFollowings = context.Follows.Where(f => f.FollowerId == currentUser.Id).ToList();

            List<User> usersThatIFollow = GetFollowings(username);
            List<Follow> myFollowingsFollowings = new List<Follow>();
            foreach(User u in usersThatIFollow)
                myFollowingsFollowings.AddRange(context.Follows.Where(f => f.FollowerId == u.Id).ToList());

            List<Follow> suggestionIds = new List<Follow>();
            List<User> suggestions = new List<User>();

            foreach(Follow f in myFollowingsFollowings)
                foreach(Follow f2 in myFollowings)
                    if (f2.FollowingId != f.FollowingId && suggestionIds.Contains(f) == false && f.FollowingId != currentUser.Id)
                        suggestionIds.Add(f);
            foreach (Follow f in suggestionIds)
                suggestions.Add(context.Users.FirstOrDefault(u => u.Id == f.FollowingId));

            return suggestions;
        }
    }
}
