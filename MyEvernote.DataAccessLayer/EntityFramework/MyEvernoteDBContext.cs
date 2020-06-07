using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyEvernoteDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public MyEvernoteDBContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // FluentAPI

            modelBuilder.Entity<Category>()
                .HasMany(x => x.Notes)
                .WithRequired(x => x.Category).WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Notes)
                .WithRequired(x => x.User).WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>()
                .HasMany(x => x.Comments)
                .WithRequired(x => x.Note).WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>()
                .HasMany(x => x.Likes)
                .WithRequired(x => x.Note).WillCascadeOnDelete(true);
        }
    }
}
