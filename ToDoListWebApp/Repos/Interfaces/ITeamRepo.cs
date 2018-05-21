using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Models;
using ToDoListWebApp.Repos.Base;

namespace ToDoListWebApp.Repos.Interfaces
{
    public interface ITeamRepo : IRepo<Team>
    {
        IEnumerable<Team> GetAllWithUsers();

        Team GetOneWithUser(Guid id);

        int RemoveFromDepartment(ApplicationUser user, Guid DepartmentId);
        int AddToDepartment(ApplicationUser user, Guid DepartmentId);
        bool UserIsInTeam(ApplicationUser user, Guid id);
    }
}
