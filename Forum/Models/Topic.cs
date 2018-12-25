﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Topic
    {
        public Topic()
        {
            this.Comments = new List<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Updated Date")]
        public DateTime LastUpdatedDate { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public List<Comment> Comments { get; set; }

        [NotMapped]
        [Display(Name = "Number Comments")]
        public int NumberComments => Comments.Count;


        public virtual ApplicationUser Author { get; set; }
    }
}
