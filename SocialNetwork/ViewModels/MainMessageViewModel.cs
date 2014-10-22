﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SocialNetwork.ViewModels
{
    public class MainMessageViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Sender")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Unread Messages")]
        public int noOfMessages { get; set; }
    }
}