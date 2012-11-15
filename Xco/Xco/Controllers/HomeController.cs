using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xco.Models;

namespace Xco.Controllers
{
    public class HomeController : Controller
    {

        private XcoContext db = new XcoContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Forward(string ShortenedUrlPath)
        {
            //create full, short-url against current
            string ShortenedUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/" + ShortenedUrlPath;
            
            //lookup in db
            var link = db.Links.FirstOrDefault(l => l.ShortenedUrl == ShortenedUrl);

            if (link == null) {
                //if link not found, show 404
                Response.StatusCode = 404;
                return View("InvalidUrl");
            }

            //log visit - todo: add the location info in there at some point
            Visit v = new Visit() { LinkId = link.LinkId, Stamp = DateTime.UtcNow };
            db.Visits.Add(v);
            db.SaveChanges();

            string redirectionUrl = link.OriginalUrl;
            //identify referrer for forwarding
            string referrer = (null != Request.UrlReferrer ? Request.UrlReferrer.ToString() : Request.Url.ToString());
            //identify referrer for forwarding
            redirectionUrl = redirectionUrl + (redirectionUrl.Contains('?') ? "&" : "?") + "Referer=" + Server.UrlEncode(referrer);

            //do redirect
            Response.RedirectPermanent(redirectionUrl);

            return null;
        }

    }
}
