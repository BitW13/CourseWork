using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Models
{
    public class UserEditDataModel
    {
        #region Модель для редактирования данных пользователя 

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Никнейм")]
        [DataType(DataType.Text)]
        [StringLength(12, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 4)]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 2)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 4)]
        public string UserSurname { get; set; }

        #endregion
    }
}