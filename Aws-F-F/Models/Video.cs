using System;
using System.ComponentModel.DataAnnotations;

namespace Aws_F_F.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? VideoPath { get; set; } // اسم الفيديو المرفوع

        [DataType(DataType.Date)]
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}
