﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Category
    {
        // inside [] is called data annotation, the line below is paramemter
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }  //? means the string can be nullable on creation*

        [Required]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100 only!!")]

        public int DisplayOrder { get; set; }

    }
}
