using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Models;
using ToDoListWebApp.Repos.Base;
using ToDoListWebApp.Repos.Interfaces;

namespace ToDoListWebApp.Repos
{
    public class SupervisorRepo : RepoBase<Supervisor>, ISupervisorRepo
    {
    }
}
