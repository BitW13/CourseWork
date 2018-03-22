using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Models
{
    public class UserGetTickets
    {
        #region Модель для получения билетов

        public int Id { get; set; }

        [Required]
        [Display(Name = "Количество купонов")]
        public int UserTickets { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        #endregion
    }
}