using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Blog
{
    public class BlogCreate
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(10, ErrorMessage = "Min length is 10 characters")]
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(300, ErrorMessage = "Min length is 300 characters")]
        [MaxLength(3000, ErrorMessage = "Max length is 3000 characters")]
        public string Content { get; set; }
        public int? PhotoId { get; set; }

    }
}
