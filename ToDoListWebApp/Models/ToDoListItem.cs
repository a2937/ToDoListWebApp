using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Exceptions;
using ToDoListWebApp.Models.Base;

namespace ToDoListWebApp.Models
{
    [Table("TodoListItem", Schema = "TodoList")]
    public class ToDoListItem : EntityBase
    {
        private String name;

        private DateTime duedate; 
        public String Name
        {
            get { return name; } 
            set
            {
                if(value != null && value != "")
                {
                    name = value; 
                }
            }
        }

        public bool Complete { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate
        {
            get
            {
                return duedate; 
            }
                
            set
            {
                if(DateTime.Now.CompareTo(value) >= 1)
                {
                    throw new InvalidDateException(" Cannot assign things to be done earlier."); 
                }
                else
                {
                    duedate = value; 
                }
            }
        }
    }
}
