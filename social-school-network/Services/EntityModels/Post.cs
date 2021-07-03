using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EntityModels
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PostId { get; set; }
        public long UserId { get; set; }
        public string PostText { get; set; }
        public string PostImagesOrVideos { get; set; }
        public long Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public long EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool IsDelete { get; set; }
        public string Feeling { get; set; }
        public string Role { get; set; }
        public long TotalLikes { get; set; }
        public long TotalComments { get; set; }
        public long TotalShares { get; set; }
    }
}
