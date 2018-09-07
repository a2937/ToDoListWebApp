using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;
using ToDoListWebApp.Repos.Interfaces;

namespace ToDoListWebApp.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        //private readonly ApplicationDbContext _context;

        private ITeamRepo _teamRepo { get; set; }
        private UserManager<ApplicationUser> _userManager;
        private ISupervisorRepo _supervisorRepo;

        public TeamsController( ITeamRepo teamRepo, UserManager<ApplicationUser> userManager, ISupervisorRepo supervisorRepo)
        {
            _teamRepo = teamRepo;
            _userManager = userManager;
            _supervisorRepo = supervisorRepo;
          //  _context = context;
        }


        // GET: Teams
        public IActionResult Index()
        {
            //var applicationDbContext = _context.Team.Include(t => t.Supervisor);
            var result = _teamRepo.GetAllWithUsers();
            //return View(await applicationDbContext.ToListAsync());
            return View(result.ToList());
        }

        // GET: Teams/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            var team = await _context.Team
                .Include(t => t.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            */
            var team = _teamRepo.GetAllWithUsers().SingleOrDefault(x => x.Id == id); 

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            //ViewData["SupervisorId"] = new SelectList(_context.Set<Supervisor>(), "Id", "Id");
            ViewData["SupervisorId"] = new SelectList(_supervisorRepo.GetAll().ToHashSet<Supervisor>(), "Id", "Id");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,SupervisorId,Id,TimeStamp")] Team team)
        public async Task<IActionResult> CreateAsync([Bind("Name,SupervisorId,Id,TimeStamp")] Team team)
        {
            if(string.IsNullOrEmpty(team.SupervisorId.ToString()))
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                _supervisorRepo.Add(new Supervisor { EmployeeId = user.Id, User = user });
                _supervisorRepo.SaveChanges();
                team.SupervisorId = _supervisorRepo.GetAll().Where(x => x.EmployeeId == user.Id).FirstOrDefault().Id;
            }
            if (ModelState.IsValid)
            {
                //team.Id = Guid.NewGuid();
                _teamRepo.Add(team);
                _teamRepo.SaveChanges(); 
                //_context.Add(team);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["SupervisorId"] = new SelectList(_context.Set<Supervisor>(), "Id", "Id", team.SupervisorId);
            ViewData["SupervisorId"] = new SelectList(_supervisorRepo.GetAll().ToHashSet<Supervisor>(), "Id", "Id", team.SupervisorId);
            
            return View(team);
        }

        // GET: Teams/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            var team = _teamRepo.GetAll().SingleOrDefault(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["SupervisorId"] = new SelectList(_supervisorRepo.GetAll().ToHashSet<Supervisor>(), "Id", "Id", team.SupervisorId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Name,SupervisorId,Id,TimeStamp")] Team team)
        public IActionResult Edit(Guid id, [Bind("Name,SupervisorId,Id,TimeStamp")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _teamRepo.Update(team);
                    _teamRepo.SaveChanges(); 
                    //_context.Update(team);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["SupervisorId"] = new SelectList(_context.Set<Supervisor>(), "Id", "Id", team.SupervisorId);
            ViewData["SupervisorId"] = new SelectList(_supervisorRepo.GetAll().ToHashSet<Supervisor>(), "Id", "Id", team.SupervisorId);
            return View(team);
        }

        // GET: Teams/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            var team = await _context.Team
                .Include(t => t.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            */
            var team = _teamRepo.GetAllWithUsers().SingleOrDefault(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        public IActionResult DeleteConfirmed(Guid id)
        {
            /*
            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            */
            var team = _teamRepo.GetAll().SingleOrDefault(m => m.Id == id);
            _teamRepo.Delete(team);
             _teamRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(Guid id)
        {
            return _teamRepo.GetAll().Any(e => e.Id == id);
        }
    }
}
