using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KRoberts_Theatre_Blog.Models
{
    public class Staff : User
    {
        [Display(Name = "Staff Type")] public StaffType StaffType { get; set; }
    }

    public enum StaffType
    {
        Admin,
        Moderator
    }
}