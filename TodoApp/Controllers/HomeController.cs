using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Persist;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TodoDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, TodoDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TodoList(string searchItem)
        {
            if (string.IsNullOrEmpty(searchItem))
            {
                var Todos = _dbContext.Todo.ToList();
                return View(Todos);
            }
            else
            {
                var TodosSearched = _dbContext.Todo.Where(
                (todo) => todo.TodoName.Contains(searchItem) || todo.AssignedPerson.Contains(searchItem)).ToList();
                return View(TodosSearched);
            }

        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Todo t)
        {
            _dbContext.Add(t);
            _dbContext.SaveChanges();
            TempData["Success"] = "New Item Created";

            return RedirectToAction("TodoList");
        }

        public IActionResult DeletePage(int id)
        {
            var item = _dbContext.Todo.FirstOrDefault(t => t.Id == id);
            return View(item);
        }

        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            var item = _dbContext.Todo.Find(id);

            _dbContext.Remove(item);
            _dbContext.SaveChanges();

            TempData["Success"] = "Todo Item Removed";

            return RedirectToAction("TodoList");
        }

        public IActionResult EditPage(int id)
        {
            var item = _dbContext.Todo.FirstOrDefault(t => t.Id == id);

            return View(item);
        }

        [HttpPost]
        public IActionResult EditItem(Todo item)
        {
            var todo = _dbContext.Todo.FirstOrDefault(t => t.Id == item.Id);
            todo.TodoName = item.TodoName;
            todo.AssignedPerson = item.AssignedPerson;

            _dbContext.SaveChanges();

            TempData["Success"] = "Item Was Sucessfully Updated";

            return RedirectToAction("TodoList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
