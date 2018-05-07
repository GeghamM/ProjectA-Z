using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class DataBaseContext: DbContext
    {
        public DataBaseContext() : base("DBConnection")
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Country> Countires { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DbSet<Review> Reviews { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<DataBaseContext>(null);
        //    base.OnModelCreating(modelBuilder);
        //}

    }
}
