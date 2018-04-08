using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Context.ContextModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public string Password { get; set; }

        public string UserRoleName { get; set; }

        public int UserCoins { get; set; }

        public int UserTickets { get; set; }
    }
}