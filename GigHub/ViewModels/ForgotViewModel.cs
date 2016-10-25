﻿using System.ComponentModel.DataAnnotations;

namespace GigHub.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }
}