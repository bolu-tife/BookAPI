using System;
using BookAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class BookApiDataContext : DbContext
    {
        public BookApiDataContext(DbContextOptions<BookApiDataContext> options) : base(options)
        {
        }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
