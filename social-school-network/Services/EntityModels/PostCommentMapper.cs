using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EntityModels
{
    public class PostCommentMapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public long EditedBy { get; set; }
        public string CommentText { get; set; }
        public bool IsDeleted { get; set; }
    }
}
