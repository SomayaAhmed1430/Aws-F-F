using System;
using System.ComponentModel.DataAnnotations;

namespace Aws_F_F.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Title { get; set; }

        [Required(ErrorMessage = "المحتوى مطلوب")]
        public string Content { get; set; }

        public string? ImagePath { get; set; } // اختيارية

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;
    }
}
