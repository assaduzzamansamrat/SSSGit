using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostLikeMapper> PostLikeMappers { get; set; }

        public DbSet<PostCommentMapper> PostCommentMappers { get; set; }

    }
}
