using System.ComponentModel.DataAnnotations;

namespace KRoberts_Theatre_Blog.Models.ViewModels
{
    public class CreateCommentViewModel
    {
        public int Id { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        
        public int PostId { get; set; }
        
        public string UserId { get; set; }
    }
}