using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Models;
using ToDoListWebApp.Repos.Base;

namespace ToDoListWebApp.Repos.Interfaces
{
    public interface IToDoItemRepo : IRepo<ToDoListItem>
    {
    }
}
