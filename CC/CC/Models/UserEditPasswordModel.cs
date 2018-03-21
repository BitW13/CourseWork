using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Models
{
    public class UserEditPasswordModel
    {
        #region Модель для редактирования пароля пользователя

        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Пороли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        #endregion
    }
}