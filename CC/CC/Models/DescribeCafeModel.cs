using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CC.Models
{
    public class DescribeCafeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название заведения")]
        [StringLength(60, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Описание")]
        [StringLength(10000, ErrorMessage = "Это поле должно быть от {0} до {1} символов", MinimumLength = 50)]
        public string Description { get; set; }
    }
}