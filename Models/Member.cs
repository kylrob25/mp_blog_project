using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KRoberts_Theatre_Blog.Models
{
    public class Member : User
    {
        [Display(Name = "Favourite Kind of Theatre")]
        public TheatreType TheatreType { get; set; }
    }

    public enum TheatreType
    {
        Comedy,
        Farce,
        Mime
    }
}