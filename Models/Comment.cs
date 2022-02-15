using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KRoberts_Theatre_Blog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? PublishDate { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}