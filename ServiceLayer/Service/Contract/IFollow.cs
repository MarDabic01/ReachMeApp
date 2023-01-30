using DomainLayer.Model;
using System.Collections.Generic;

namespace ServiceLayer.Service.Contract
{
    public interface IFollow
    {
        void FollowUser(int followerId, int followingId);
        void UnfollowUser(int followerId, int followingId);
        bool IsAlreadyFollowing(int followerId, int followingId);
        List<User> GetFollowers(User user);
        List<User> GetFollowings(User user);
        List<User> GetSuggestions(User user);
    }
}
