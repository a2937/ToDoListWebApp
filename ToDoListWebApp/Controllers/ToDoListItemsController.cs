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
    public class ToDoListItemsController : Controller
    {
        private readonly IToDoItemRepo _toDoRepo;
        private readonly UserManager<ApplicationUser> _userManager; 
        public ToDoListItemsController(IToDoItemRepo toDoRepo, UserManager<ApplicationUser> userManager)
        {
            _toDoRepo = toDoRepo;
            _userManager = userManager; 
        }

        // GET: ToDoListItems
        //public async Task<IActionResult> Index()
        public async Task<IActionResult> Index()
        {
            //return View(await _toDoRepo.GetAll().ToListAsync());
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(_toDoRepo.GetAll().Where(x => x.UserId == user.Id).ToList());
        }

        // GET: ToDoListItems/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var toDoListItem = await _toDoRepo.SingleOrDefaultAsync(m => m.Id == id);
            var toDoListItem = _toDoRepo.GetAll().SingleOrDefault(m => m.Id == id); 
            if (toDoListItem == null)
            {
                return NotFound();
            }

            return View(toDoListItem);
        }

        // GET: ToDoListItems/Create
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.UserId = user.Id;
            ViewBag.TempDueDate = DateTime.UtcNow.AddDays(7);
            return View();
        }

        // POST: ToDoListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Complete,DueDate,Id,TimeStamp")] ToDoListItem toDoListItem)
        public IActionResult Create([Bind("Name,Complete,DueDate,Id,TimeStamp,UserId")] ToDoListItem toDoListItem)
        {
            if (ModelState.IsValid)
            {
                toDoListItem.Id = Guid.NewGuid();
                _toDoRepo.Add(toDoListItem);
                _toDoRepo.SaveChanges(); 
                //await _toDoRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoListItem);
        }

        // GET: ToDoListItems/Edit/5
        // public async Task<IActionResult> Edit(Guid? id)
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var toDoListItem = await _toDoRepo.SingleOrDefaultAsync(m => m.Id == id);
            var toDoListItem =  _toDoRepo.GetAll().SingleOrDefault(m => m.Id == id);
            if (toDoListItem == null)
            {
                return NotFound();
            }
            return View(toDoListItem);
        }

        // POST: ToDoListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Name,Complete,DueDate,Id,TimeStamp")] ToDoListItem toDoListItem)
        public IActionResult Edit(Guid id, [Bind("Name,Complete,DueDate,Id,TimeStamp")] ToDoListItem toDoListItem)
        {
            if (id != toDoListItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _toDoRepo.Update(toDoListItem);
                    _toDoRepo.SaveChanges(); 
                    //await _toDoRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoListItemExists(toDoListItem.Id))
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
            return View(toDoListItem);
        }

        // GET: ToDoListItems/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var toDoListItem = await _toDoRepo.GetAll().SingleOrDefaultAsync(m => m.Id == id);
            var toDoListItem =  _toDoRepo.GetAll().SingleOrDefault(m => m.Id == id);

            if (toDoListItem == null)
            {
                return NotFound();
            }

            return View(toDoListItem);
        }

        // POST: ToDoListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(Guid id)
        public IActionResult DeleteConfirmed(Guid id)
        {
            //var toDoListItem = await _toDoRepo.ToDoListItem.SingleOrDefaultAsync(m => m.Id == id);
            var toDoListItem =  _toDoRepo.GetAll().SingleOrDefault(m => m.Id == id);
            _toDoRepo.Delete(toDoListItem);
            _toDoRepo.SaveChanges(); 
            //await _toDoRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoListItemExists(Guid id)
        {
            return _toDoRepo.GetAll().Any(e => e.Id == id);
        }
    }
}
