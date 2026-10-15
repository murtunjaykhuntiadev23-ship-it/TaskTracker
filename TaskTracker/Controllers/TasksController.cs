using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;

namespace TaskTracker.Controllers;

public class TasksController : Controller
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    //// Shows all tasks
    //public async Task<IActionResult> Index()
    //{
    //    var tasks = await _context.Tasks
    //        .OrderByDescending(t => t.CreatedAt)
    //        .ToListAsync();
    //    return View(tasks);
    //}

    // Shows all tasks, with optional search
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Title.Contains(search) ||
                                     (t.Description != null && t.Description.Contains(search)));
        }

        ViewData["Search"] = search;

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return View(tasks);
    }


    // Shows the empty form
    public IActionResult Create()
    {
        return View();
    }

    // Saves the form data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] TaskItem task)
    {
        if (!ModelState.IsValid)
            return View(task);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Shows the edit form
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    // Saves the changes
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItem input)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        if (!ModelState.IsValid) return View(input);

        task.Title = input.Title;
        task.Description = input.Description;
        task.Status = input.Status;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Asks "are you sure?"
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    // Deletes the task
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}