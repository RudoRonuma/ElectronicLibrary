﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase.Models
{
    public class LibraryUser
    {
        [Key]
        public long UsertId { get; set; }

        [MaxLength(45)]
        [MinLength(2)]
        public string Name { get; set; }

        [MaxLength(32)]
        [MinLength(3)]
        public string Username { get; set; }

        [MaxLength(32)]
        [MinLength(4)]
        public string Email { get; set; }

        [MaxLength(64)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}