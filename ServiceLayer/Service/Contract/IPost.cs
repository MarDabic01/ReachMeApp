using DomainLayer.Model;
using System.Collections.Generic;

namespace ServiceLayer.Service.Contract
{
    public interface IPost
    {
        Post SelectPost(int postId);
        User GetAuthor(int userId);
        void LikePost(int postId, int userId);
        void UnlikePost(int postId, int userId);
        bool IsPostLiked(int postId, int userId);
        int NumberOfLikes(int postId);
        void PostComment(int postId, int userId, string comment);
        List<Comment> GetComments(int postId);
    }
}
