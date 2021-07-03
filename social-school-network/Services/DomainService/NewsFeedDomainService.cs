using Services.DataService;
using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainService
{
    public class NewsFeedDomainService
    {
        NewsFeedDataService newsFeedDataService = new NewsFeedDataService();


        public List<Post> GetAllUsersData()
        {
            List<Post> post = new List<Post>();
           
            try
            {
                post = newsFeedDataService.GetAll();
                return post;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Post GetUserByID(int id)
        {
            return newsFeedDataService.GetPostByUserId(id);
        }

        public bool CreateNewPost(Post post, long userId, string userRole)
        {
            try
            {
                return newsFeedDataService.CreateNewPost(post, userId,userRole);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public long InsertOrUpdateLikeStatusByUserIdAndPostId(long userId, long postId, bool isLiked)
        {
            try
            {
                return newsFeedDataService.InsertOrUpdateLikeStatusByUserIdAndPostId(userId,postId,isLiked);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public long InsertOrUpdateComment(long commentId, string commentText, long userId, long postId)
        {
            try
            {
                return newsFeedDataService.InsertOrUpdateComment(commentId, commentText, userId, postId);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<PostCommentMapper> GetAllCommentsByPostId(long postId)
        {

            try
            {
                return newsFeedDataService.GetAllCommentsByPostId(postId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
