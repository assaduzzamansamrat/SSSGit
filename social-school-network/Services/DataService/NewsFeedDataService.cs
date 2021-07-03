using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataService
{
    public class NewsFeedDataService
    {
        AppDbContext context = new AppDbContext();

        public List<Post> GetAll()
        {
            try
            {
                return context.Posts.OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Post GetPostByUserId(int id)
        {
            try
            {
                return context.Posts.SingleOrDefault(d => d.Createdby == id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool CreateNewPost(Post post,long userId, string userRole)
        {
            bool isPostAddTrue = false;
            post.Createdby = userId;
            post.UserId = userId;
            post.CreatedDate = DateTime.Now;
            post.EditedDate = DateTime.Now;
            post.IsDelete = false;
            post.Role = "Student";
            try
            {
                context.Posts.Add(post);
                context.SaveChanges();
                isPostAddTrue = true;
                return isPostAddTrue;
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
                long totalLikes = 0;
                PostLikeMapper likeMapper = context.PostLikeMappers.Where(x => x.UserId == userId && x.PostId == postId).FirstOrDefault();
                if(likeMapper != null)
                {
                    likeMapper.IsLiked = isLiked;
                    context.SaveChanges();
                }
                else
                {
                    PostLikeMapper likeMapperToInsert = new PostLikeMapper();
                    likeMapperToInsert.UserId = userId;
                    likeMapperToInsert.PostId = postId;
                    likeMapperToInsert.IsLiked = isLiked;
                    likeMapperToInsert.CreatedBy = userId;
                    likeMapperToInsert.CreatedDate = DateTime.Now;
                    likeMapperToInsert.EditedBy = userId;
                    likeMapperToInsert.EditedDate = DateTime.Now;
                    context.PostLikeMappers.Add(likeMapperToInsert);
                    context.SaveChanges();
                }
                totalLikes = context.PostLikeMappers.Where(x => x.PostId == postId && x.IsLiked == true).ToList().Count();
                UpdateTotalLikesForPostById(postId, totalLikes);

                return totalLikes;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool UpdateTotalLikesForPostById(long postId, long totalLikes)
        {
            try
            {
                Post post = context.Posts.Where(x => x.PostId == postId).FirstOrDefault();
                if (post != null)
                {
                    post.TotalLikes = totalLikes;
                    context.SaveChanges();
                    return true;
                }
                return false;
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
                List<PostCommentMapper> commentList = new List<PostCommentMapper>();
            
                commentList = context.PostCommentMappers.Where(x => x.PostId == postId).OrderByDescending(x => x.CreatedDate).ToList();
                return commentList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public long InsertOrUpdateComment(long commentId, string commentText, long userId,long postId)
        {
            try
            {
                PostCommentMapper postCommentMapper = context.PostCommentMappers.Where(x=> x.Id == commentId).FirstOrDefault();
                if(postCommentMapper != null)
                {
                    postCommentMapper.CommentText = commentText;
                    postCommentMapper.EditedBy = userId;
                    postCommentMapper.EditedDate = DateTime.Now;
                    context.SaveChanges();
                    return commentId;
                }
                else
                {
                    postCommentMapper = new PostCommentMapper();
                    postCommentMapper.CreatedBy = userId;
                    postCommentMapper.CreatedDate = DateTime.Now;
                    postCommentMapper.PostId = postId;
                    postCommentMapper.UserId = userId;
                    postCommentMapper.EditedDate = DateTime.Now;
                    postCommentMapper.EditedBy = userId;
                    postCommentMapper.CommentText = commentText;
                    context.PostCommentMappers.Add(postCommentMapper);
                    context.SaveChanges();
                    return postCommentMapper.Id;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}
