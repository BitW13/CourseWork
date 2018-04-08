using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CC.Context.ContextModels;

namespace CC.Models
{
    public class UserGetRightsModel
    {
        #region Модель для получения прав

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Защитный код")]
        [DataType(DataType.Password)]
        public string SecurityCode { get; set; }

        #endregion
    }
}