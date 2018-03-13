using System;
using System.Collections.Generic;
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
        public DbSet<Cafe> Cafes { get; set; }

    }
    #endregion

    #region Модель для создания таблицы в БД (Пользователь)

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

    #endregion

    #region Модель для создания таблицы в БД (Новости)

    public class Record
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    #endregion

    #region Модель для создания таблицы в БД (Описание заведений)

    public class Cafe
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion
}