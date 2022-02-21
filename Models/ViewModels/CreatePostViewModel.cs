using System.ComponentModel.DataAnnotations;

namespace KRoberts_Theatre_Blog.Models.ViewModels
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        
        public bool Published { get; set; }
        
        public int CategoryId { get; set; }
    }
}