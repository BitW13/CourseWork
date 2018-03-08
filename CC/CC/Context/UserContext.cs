using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CC.Context
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Password { get; set; }
        public string UserRoleName { get; set; }
        public int UserCoins { get; set; }
        public int UserTickets { get; set; }
    }

    public class Record
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}