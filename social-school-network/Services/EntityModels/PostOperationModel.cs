using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EntityModels
{
    public class PostOperationModel
    {
        public List<Post> Post { get; set; }
        public List<string> PostImagesOrVideos { get; set; }
        public List<User> users { get; set; }
        public List<PostLikeMapper> PostLikeMappers { get; set; }
       
    }
}
