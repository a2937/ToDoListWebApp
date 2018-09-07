using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;
using ToDoListWebApp.Repos.Base;
using ToDoListWebApp.Repos.Interfaces;

namespace ToDoListWebApp.Repos
{
    public class TeamRepo : RepoBase<Team>, ITeamRepo
    {
        private UserManager<ApplicationUser> _userManager;
        private DbContextOptions<ApplicationDbContext> options;

        public TeamRepo() : base()
        {
        }

        public TeamRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.options = options; 
        }

        public TeamRepo(DbContextOptions<ApplicationDbContext> options, UserManager<ApplicationUser> userManager) : base(options)
        {
            _userManager = userManager;
        }

        public override IEnumerable<Team> GetRange(int skip, int take)
     => GetRange(Table.OrderBy(x => x.Name), skip, take);

        public IEnumerable<Team> GetAllWithUsers()
        => Table.Include(x => x.AssignedEmployees).Include(x => x.Supervisor);

        public override IEnumerable<Team> GetAll()
           => Table.OrderBy(x => x.Name);

        public Team GetOneWithUser(Guid id)
       => Table.Include(x => x.AssignedEmployees).FirstOrDefault(x => x.Id == id);

        public int RemoveFromDepartment(ApplicationUser user, Guid DepartmentId)
        {
            if (user == null)
            {
                return 0;
            }
            user.TeamId = null;
            Team department = Table.Include(x => x.AssignedEmployees).Where(x => x.Id == DepartmentId).First();

            SaveChanges();
            if (department.AssignedEmployees.Contains(user))
            {
                department.AssignedEmployees.Remove(user);

                SaveChanges();

                //department.AssignedEmployees.Add(user);
                //SaveChanges();
            }
            return department.AssignedEmployees.Contains(user) ? 0 : 1;
            //department.AssignedEmployees.Remove(user);
            //user.DepartmentId = null; 
            //return SaveChanges();
        }

        public int AddToDepartment(ApplicationUser user, Guid DepartmentId)
        {
            if (user == null)
            {
                return 0;
            }
            Team department = Table.Include(x => x.AssignedEmployees).Where(x => x.Id == DepartmentId).First();
            // Department department = Table.Find(DepartmentId);

            if (department != null)
            {
                user.TeamId = DepartmentId;

                SaveChanges();
                if (department.AssignedEmployees.Contains(user) == false)
                {
                    // department.AssignedEmployees.Remove(user);

                    // SaveChanges();

                    department.AssignedEmployees.Add(user);

                    SaveChanges();
                }
                return department.AssignedEmployees.Contains(user) ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }

        public bool UserIsInTeam(ApplicationUser user, Guid id)
        {
            return user.TeamId.Equals(id);
            //return
        }
    }
}
