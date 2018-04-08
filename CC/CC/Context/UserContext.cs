using CC.Context.ContextModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CC.Context
{
    #region Контекст для работы с БД
    public class UserContext : DbContext
    {

        public UserContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Cafe> Cafes { get; set; }

    }
    #endregion
}