using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Controllers
{
    public class TopicController : Controller
    {
        private readonly ForumDbContext context;
        
        public TopicController(ForumDbContext context)
        {
            this.context = context;
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Topic topic = this.context.Topics
                .Include(t => t.Author)
                .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
                .Where(t => t.Id == id)
                .SingleOrDefault();

            if (topic == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(topic);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.CreatedDate = DateTime.Now;
                topic.LastUpdatedDate = DateTime.Now;

                string authorId = context.Users
                    .Where(u => u.UserName == User.Identity.Name)
                    .First()
                    .Id;

                topic.AuthorId = authorId;

                this.context.Topics.Add(topic);
                context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(topic);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var topic = this.context.Topics
                .Include(t => t.Author)
                .SingleOrDefault(m => m.Id == id);

            if (topic == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(topic);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Topic topic = this.context.Topics
                .Include(t => t.Author)
                .SingleOrDefault(m => m.Id == id);

            if (topic != null)
            {
                this.context.Topics.Remove(topic);
                this.context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Topic topic = context.Topics.Include(t => t.Author).Where(t => t.Id == id).SingleOrDefault();

            if (topic == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(topic);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {
                Topic topicFromDb = context.Topics.Include(t => t.Author).SingleOrDefault(t => t.Id.Equals(topic.Id));

                if (topicFromDb == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                topicFromDb.Description = topic.Description;
                topicFromDb.Title = topic.Title;

                topicFromDb.LastUpdatedDate = DateTime.Now;

                context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(topic);
        }

    }
}