using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MovieWebApp.Models;

namespace MovieWebApp.Controllers
{
    [Authorize]
    public class MoviesController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            if (page < 1) page = 1;
            int skip = (page - 1) * 10;

            var movies = Db.Movies.OrderBy(x => x.ID).Skip(skip).Take(10).ToList();

            ViewBag.PageCurrent = page;
            ViewBag.Pages = GetPages(page);
            return View(movies);
        }


        // Get  
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Movies movie, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                movie.UserID = CurrentUserId;
                movie.Poster = image.FileName;
                Db.Movies.Add(movie);
                Db.SaveChanges();

                if (image.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(image.FileName);
                    image.SaveAs(Server.MapPath("~/Files/" + fileName));
                }
            }
            return View(movie);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var movie = Db.Movies.SingleOrDefault(e => e.ID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        [HttpPost]
        public ActionResult Edit(Movies movie, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var movieOrigin = Db.Movies.First(x => x.ID == movie.ID);

                if (movieOrigin.UserID != CurrentUserId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                movieOrigin.Description = movie.Description;
                movieOrigin.Producer = movie.Producer;
                movieOrigin.Year = movie.Year;
                movieOrigin.Name = movie.Name;

                //We dont delete old posters for simplification because otherwise we must make sure this poster not using in other movies
                if (image != null)
                {
                    string fileName = System.IO.Path.GetFileName(image.FileName);
                    image.SaveAs(Server.MapPath("~/Files/" + fileName));
                    movieOrigin.Poster = image.FileName;
                }

                Db.SaveChanges();
            }
            return View(movie);
        }


        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var movie = Db.Movies.SingleOrDefault(e => e.ID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var movie = Db.Movies.SingleOrDefault(e => e.ID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var movie = Db.Movies.SingleOrDefault(x => x.ID == id);
            Db.Movies.Remove(movie ?? throw new InvalidOperationException());
            Db.SaveChanges();
            return RedirectToAction("Index");
        }


        private List<int> GetPages(int page)
        {

            int minPage = page;

            while ((minPage - 1) % 5 > 0)
            {
                minPage--;
            }

            List<int> range = Enumerable.Range(minPage, 5).ToList();

            return range;
        }
    }
}