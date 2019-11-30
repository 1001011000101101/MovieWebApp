using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MovieWebApp.Models;

namespace MovieWebApp.Controllers
{
    public class BaseController : Controller
    {
        private MoviesEntities db;

        public MoviesEntities Db
        {
            get
            {
                if (db == null)
                    db = new MoviesEntities();

                return db;
            }
        }

        public string CurrentUserId
        {
            get
            {
                string userId = string.Empty;

                if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                {
                    userId = System.Web.HttpContext.Current.User.Identity.GetUserId<string>();
                }

                return userId;
            }
        }






    }
}