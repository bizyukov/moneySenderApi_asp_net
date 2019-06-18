using dotnetWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetWebApi
{
    public class AppDataBaseContext : DbContext
    {
        public AppDataBaseContext(DbContextOptions<AppDataBaseContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
