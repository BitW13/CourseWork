using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Models
{
    public class RecordAddModel
    {
        #region Модель для добавления новости

        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Никнейм")]
        [DataType(DataType.Text)]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок")]
        [StringLength(600, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 10)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Описание")]
        [StringLength(10000, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 50)]
        public string Description { get; set; }

        #endregion
    }
}