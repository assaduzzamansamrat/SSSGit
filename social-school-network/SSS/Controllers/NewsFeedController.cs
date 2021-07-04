using Services.DomainService;
using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSS.Controllers
{
    [Authorize(Roles = "Student")]
    public class NewsFeedController : Controller
    {
        // GET: NewsFeed
        NewsFeedDomainService newsFeedDomainService;
        UserDomainService userDomainService;

        string activeMember = "<a class='list-group-item list-group-item-action custom-list-group-item dummyopenchat' image-path='###imagepath###' onclick='OpenChatBox(this)' user-name='###username###' userId='###userid###' data-toggle='modal' data-target='#chatModal' > " +
                                "<img src = '###imagepath###' class='chat-icon' alt='###alt###' />" +
                                "<span class='user-image-name'>###username###</span>" +
                                "###dot###" +
                               "</a>";

        string commentDynamicHtml = "<div id='coments_###CommentId###' commentId='###CommentId###'>" +
                             "<img src ='###UserProfileImage###' class='chat-icon' alt='games'>" +
                             "<span style='padding-left:5px;'>###UserFullName###</span>###settingsicon###<div class='chat-settings-box' id='settingbox_###CommentId###' style='display:none;'></div>" +
                             "<br/>" +
                             "<div class='comment-container'>###CommentText###</div>" +
                             "</div><br/>";
        public NewsFeedController()
        {
            newsFeedDomainService = new NewsFeedDomainService();
            userDomainService = new UserDomainService();
        }
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                long userId = (long)Session["UserId"];
                PostOperationModel postoperationModel = new PostOperationModel();
                postoperationModel.Post = newsFeedDomainService.GetAllUsersData();
                postoperationModel.users = userDomainService.GetAllUsersData();
                postoperationModel.PostLikeMappers = userDomainService.GetAllPostLikeMapperListByUserId(userId);
                ViewBag.userId = userId;
                return View(postoperationModel);

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public ActionResult GetUserImageAndNameByUserId()
        {
            try
            {
                long userId = (long)Session["UserId"];
                User user = userDomainService.GetUserByID(userId);
                return Json(user, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult GetAllUsersByUserId()
        {
            try
            {
                long userId = -1;
                if (Session["UserId"] != null && (long)Session["UserId"] > 0)
                {
                     userId = (long)Session["UserId"];
                }
                List<User> users = new List<User>();

                string activeMembersPanel = "";
                users = userDomainService.GetAllActiveUsersByID(userId);
                if (users != null)
                {
                    foreach (var item in users)
                    {
                        string activeMembers = activeMember;
                        string username = item.FirstName + " " + item.LastName;
                        string alt = item.FirstName;
                        string imagePath;
                        string dot;
                        if (item.ImagePath != null)
                        {
                            imagePath = "/Images/User-Profile-Images/"+item.ImagePath;
                        }
                        else
                        {
                            imagePath = "/Images/User-Profile-Images/DefaultUserImage.jpg";
                        }

                        if (item.IsUserLoggedIn == true)
                        {
                            // dot = "<img src='/Images/active-member.png' class='chat-icon' alt='games'>";
                            dot = "<span class='dot'></span>";
                        }
                        else
                        {
                            dot = "";
                        }

                        activeMembers = activeMembers.Replace("###dot###", dot);
                        activeMembers = activeMembers.Replace("###imagepath###", imagePath);
                        activeMembers = activeMembers.Replace("###alt###", alt);
                        activeMembers = activeMembers.Replace("###username###", username);
                        activeMembers = activeMembers.Replace("###userid###", userId.ToString());
                        activeMembersPanel = activeMembersPanel + activeMembers;
                    }
                }
                return Json(activeMembersPanel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult UploadFiles()
        {

            try
            {
                string fileName = "";
                string fileNames = "";
                string path = Server.MapPath("~/Content/Upload/");
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    Guid guid = Guid.NewGuid();
                    HttpPostedFileBase file = files[i];
                    string extension = Path.GetExtension(file.FileName);
                    fileName = guid.ToString() + extension;
                    file.SaveAs(path + fileName);
                    if (i == 0)
                    {
                        fileNames = fileNames + fileName;
                    }
                    else
                    {
                        fileNames = fileNames + "," + fileName;
                    }
                }
                return Json(fileNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpPost]
        public ActionResult UploadWallpaper()
        {

            try
            {
                long userId = -1;
                if (Session["UserId"] != null && (long)Session["UserId"] > 0)
                {
                    userId = (long)Session["UserId"];
                }
                string fileName = "";
                string path = Server.MapPath("~/Content/Upload/");
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    Guid guid = Guid.NewGuid();
                    HttpPostedFileBase file = files[i];
                    string extension = Path.GetExtension(file.FileName);
                    fileName = guid.ToString() + extension;
                    file.SaveAs(path + fileName);
                    userDomainService.UpdateUserWallPaperByUserId(userId, fileName);
                    Session["WallPaper"] = fileName;

                }
                return Json(fileName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult UploadUserProfileImage()
        {

            try
            {
                long userId = -1;
                if (Session["UserId"] != null && (long)Session["UserId"] > 0)
                {
                    userId = (long)Session["UserId"];
                }
                string fileName = "";
                string path = Server.MapPath("~/Images/User-Profile-Images/");
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    Guid guid = Guid.NewGuid();
                    HttpPostedFileBase file = files[i];
                    string extension = Path.GetExtension(file.FileName);
                    fileName = guid.ToString() + extension;
                    file.SaveAs(path + fileName);
                    userDomainService.UpdateUserProfilePicture(userId, fileName);
                    Session["UserProfileImage"] = fileName;
                }
                return Json(fileName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Post(Post post)
        {
            try
            {
                string userRole = Session["Role"].ToString();
                long userid = (long)Session["UserId"];
                newsFeedDomainService.CreateNewPost(post, userid, userRole);
                return RedirectToAction("Index", "NewsFeed");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult UpdateLikeStatus(long postId, bool isLiked)
        {
            try
            {
                long totalLike = 0;
                long userId = (long)Session["UserId"];
                totalLike = newsFeedDomainService.InsertOrUpdateLikeStatusByUserIdAndPostId(userId, postId, isLiked);
                return Json(totalLike, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult InsertOrUpdateComment(long commentId, string commentText, long postId)
        {
            try
            {
                long userId = (long)Session["UserId"];
                long insertedId = 0;
                string commentHtml = commentDynamicHtml;
                insertedId = newsFeedDomainService.InsertOrUpdateComment(commentId, commentText, userId, postId);
                if(insertedId >0)
                {
                    string settingsIcon = "<i class='fas fa-ellipsis-h comment-right-settings' id='seetingsIcon_" + insertedId.ToString() + "' commentId='" + insertedId.ToString() + "' aria-hidden='true'></i>";
                    commentHtml = commentHtml.Replace("###CommentId###", insertedId.ToString());
                    if (Session["UserProfileImage"] != null)
                    {
                        commentHtml = commentHtml.Replace("###UserProfileImage###", "/Images/User-Profile-Images/" + Session["UserProfileImage"].ToString());
                    }
                    else
                    {                  
                      commentHtml = commentHtml.Replace("###UserProfileImage###", "/Images/User-Profile-Images/DefaultUserImage.jpg");                      
                    }
                    commentHtml = commentHtml.Replace("###UserFullName###", Session["UserFullName"].ToString());
                    commentHtml = commentHtml.Replace("###CommentText###", commentText);
                    commentHtml = commentHtml.Replace("###settingsicon###", settingsIcon);
                    return Json(commentHtml, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet]
        public ActionResult GetAllCommentsByPostId(long postId)
        {
            try
            {
                string allComments = "";
                long userId = (long)Session["UserId"];
                List<PostCommentMapper> commentsList = new List<PostCommentMapper>();
                commentsList = newsFeedDomainService.GetAllCommentsByPostId(postId);
                User user = new User();
                 if (commentsList != null && commentsList.Count>0)
                {
                    foreach (var item in commentsList)
                    {
                        string settingsIcon = "<i class='fas fa-ellipsis-h comment-right-settings' id='seetingsIcon_" + item.Id.ToString() + "' commentId='" + item.Id.ToString() + "' aria-hidden='true'></i>";
                        string commentHtml = commentDynamicHtml;
                        user = userDomainService.GetUserByID(item.CreatedBy);
                        commentHtml = commentHtml.Replace("###CommentId###", item.Id.ToString());
                        if (user.ImagePath != null && user.ImagePath != "")
                        {
                            commentHtml = commentHtml.Replace("###UserProfileImage###", "/Images/User-Profile-Images/" + user.ImagePath.ToString());
                        }
                        else
                        {
                            commentHtml = commentHtml.Replace("###UserProfileImage###", "/Images/User-Profile-Images/DefaultUserImage.jpg");
                        }
                        commentHtml = commentHtml.Replace("###UserFullName###",user.FirstName.ToString()+" "+user.LastName.ToString());
                        commentHtml = commentHtml.Replace("###CommentText###", item.CommentText);
                        if(userId == item.CreatedBy)
                        {
                            commentHtml = commentHtml.Replace("###settingsicon###", settingsIcon);
                        }
                        else
                        {
                           commentHtml = commentHtml.Replace("###settingsicon###", "");
                        }
                        allComments = allComments + commentHtml;
                    }
                    return Json(allComments, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}