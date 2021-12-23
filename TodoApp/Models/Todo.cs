using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string TodoName { get; set; }

        [Required, MaxLength(100)]
        public string AssignedPerson { get; set; }
    }
}
