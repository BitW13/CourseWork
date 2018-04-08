using CC.Context.ContextModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Models
{
    public class UserGetCoins
    {
        #region Модель для получения валюты 

        public Guid Id { get; set; }

        [Display(Name = "Количество Coffee-Coins")]
        public int UserCoins { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Промокод")]
        public string SecretKey { get; set; }

        #endregion
    }
}