using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CC.Models
{
    public class UserGetRightsModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Никнейм")]
        [DataType(DataType.Text)]
        [StringLength(12, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 4)]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 2)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 4)]
        public string UserSurname { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Защитный код")]
        [DataType(DataType.Password)]
        public string SecurityCode { get; set; }
    }
}