using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Models.Base;

namespace ToDoListWebApp.Models
{
    [Table("Teams", Schema = "TodoList")]
    public class Team : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public Guid? SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public Supervisor Supervisor { get; set; }

        [InverseProperty(nameof(ApplicationUser.Department))]
        public List<ApplicationUser> AssignedEmployees { get; set; } = new List<ApplicationUser>();

    }
}
