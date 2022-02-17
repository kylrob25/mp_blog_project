using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KRoberts_Theatre_Blog.Models
{
    public class Category
    {
        [Key] public int CategoryId { get; set; }

        [Display(Name = "Category")] public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }
}