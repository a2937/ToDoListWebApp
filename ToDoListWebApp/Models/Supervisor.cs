using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Models.Base;

namespace ToDoListWebApp.Models
{
    [Table("Supervisors", Schema = "TodoList")]
    public class Supervisor : EntityBase
    {
        public string EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public ApplicationUser Manager { get; set; }

        [InverseProperty(nameof(Team.Supervisor))]
        public List<Team> Teams { get; set; } = new List<Team>();

    }
}
