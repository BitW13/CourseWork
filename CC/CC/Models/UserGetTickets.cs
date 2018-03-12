﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Models
{
    public class UserGetTickets
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Количество билетов")]
        public int UserTickets { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }
    }
}