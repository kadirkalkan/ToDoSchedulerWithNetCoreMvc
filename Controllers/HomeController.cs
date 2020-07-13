using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PitonProject.Models;
using PitonProject.Models.Entities;
using PitonProject.Models.Enums;
using PitonProject.Models.Managers;
using PitonProject.Models.ViewModels;

namespace PitonProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        External external = new External();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(EnumClass.ProcessType @enum = EnumClass.ProcessType.initialize, string processMessage = "")
        {
            fillViewBag();
            setProcess(@enum, processMessage);
            return View(getEmptyTask());
        }
        [HttpPost]
        public IActionResult Index(TaskClass newTask)
        {
            fillViewBag();
            try
            {
                using (var context = new DatabaseContext())
                {
                    string loggedUserName = ViewBag.user;
                    newTask.TaskType = context.TaskTypes.FirstOrDefault(x => x.TaskTypeText == newTask.TaskType.TaskTypeText);
                    newTask.CreatedUser = context.Users.FirstOrDefault(x => x.UserName == loggedUserName);
                    switch (newTask.TaskType.TaskTypeText)
                    {
                        case "day": newTask.EndDate = newTask.StartDate.AddDays(1); break;
                        case "week": newTask.EndDate = newTask.StartDate.AddDays(7); break;
                        case "month": newTask.EndDate = newTask.StartDate.AddMonths(1); break;
                    }
                    context.Tasks.Add(newTask);
                    context.SaveChanges();
                }
                setProcess(EnumClass.ProcessType.success, "Görev Kaydı Başarılı.");
                return View(getEmptyTask());
            }
            catch (Exception ex)
            {
                external.addLog(EnumClass.LogType.error, ex.ToString(), "Index Post", "Home");
                setProcess(EnumClass.ProcessType.danger, "Bir Hatayla Karşılaşıldı");
            }
            return View(newTask);
        }

        private TaskClass getEmptyTask()
        {
            return new TaskClass()
            {
                TaskType = new TaskType()
                {
                    TaskTypeText = EnumClass.TaskType.day.ToString()
                }
            };
        }

        public IActionResult Tasks(EnumClass.ProcessType @enum = EnumClass.ProcessType.initialize, string processMessage = "")
        {
            fillViewBag();
            setProcess(@enum, processMessage);
            using (var context = new DatabaseContext())
            {
                
                ICollection<TaskClass> taskListForView = context.Tasks.Include(user => user.CreatedUser).Include(tasktype=> tasktype.TaskType).Where(x => x.CreatedUser.ID == getLoggedUser().ID).ToList();
                TasksViewModel tasksView = new TasksViewModel() { taskList = taskListForView };
                ViewBag.SearchDate = DateTime.Now.Date.ToString("dd MMM yyyy");
                return View(tasksView);
            }
        }

        [HttpPost]
        public IActionResult Tasks(DateTime datepicker)
        {
            fillViewBag();
            using (var context = new DatabaseContext())
            {
                ICollection<TaskClass> taskListForView = context.Tasks.Include(user => user.CreatedUser).Include(tasktype => tasktype.TaskType).Where(x=> x.StartDate == datepicker && x.CreatedUser.ID == getLoggedUser().ID).ToList();
                TasksViewModel tasksView = new TasksViewModel() { taskList = taskListForView };
                ViewBag.SearchResult = datepicker.ToShortDateString() + " Tarihli Kayıtlar Görüntüleniyor.";
                ViewBag.SearchDate = datepicker.Date.ToString("dd MMM yyyy");
                return View(tasksView);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private void fillViewBag()
        {
            var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            ViewBag.user = name;
        }
        private User getLoggedUser()
        {
            var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            using (var context = new DatabaseContext())
            {
                return context.Users.FirstOrDefault(x => x.UserName.Equals(name));
            }
        }

        public void setProcess(Models.Enums.EnumClass.ProcessType processType, string processMessage = null, bool waitParameter = false)
        {
            try
            {
                if (processType != Models.Enums.EnumClass.ProcessType.initialize)
                {
                    if (processType == Models.Enums.EnumClass.ProcessType.danger && String.IsNullOrEmpty(processMessage))
                    {
                        processMessage = "Bir hata meydana geldi lütfen yetkiliye bildirin.";
                    }
                    ViewBag.processType = processType.ToString();
                    ViewBag.processMessage = processMessage;
                    if (waitParameter)
                    {
                        ViewBag.processWait = 3000;
                    }
                }
            }
            catch (Exception ex)
            {
                external.addLog(Models.Enums.EnumClass.LogType.error, ex.ToString(), "UserController", "setProcess");
            }
        }

    }
}
