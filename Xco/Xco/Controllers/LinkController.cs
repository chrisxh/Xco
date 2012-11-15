using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Xco.Models;

namespace Xco.Controllers
{
    /// <summary>
    /// Simple controller to do basic retrieval and add links
    /// </summary>
    public class LinkController : ApiController
    {
        private XcoContext db = new XcoContext();

        #region Get collections and singles
        [HttpGet]
        [ActionName("All")]
        public IEnumerable<Link> GetAllLinks(){
            return db.Links.OrderByDescending(l=>l.CreatedOn).AsEnumerable();
        }

        // GET api/Link/Active
        [HttpGet]
        [ActionName("Active")]
        public IEnumerable<Link> GetActiveLinks()
        {
            return db.Links.Where(l =>
                                    !l.ActivationDate.HasValue && (!l.DeactivationDate.HasValue || l.DeactivationDate.Value >= DateTime.Now)  ||
                                    l.ActivationDate.Value <= DateTime.Now && (!l.DeactivationDate.HasValue || l.DeactivationDate.Value >= DateTime.Now)
                                  )
                           .OrderByDescending(l => l.CreatedOn).AsEnumerable();
        }

        
        // GET api/Link/5
        public Link GetLinkById(int id)
        {
            Link link = db.Links.Find(id);
            if (link == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return link;
        }

        
        [HttpGet]
        [ActionName("Original")]
        public IEnumerable<Link> GetLinkByOriginal(string Contains)
        {
            return db.Links.Where(l => l.OriginalUrl.Contains(Contains)).AsEnumerable();
        }

        #endregion

        #region Set links
        
        /// <summary>
        /// POST api/Link 
        /// Create shortened url from link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Add")]
        public HttpResponseMessage AddLink(Link link)
        {
            if (ModelState.IsValid)
            {
                //hosting protocol & domain matches this app root.  Does not take into consideration any fun routing
                Uri CurrentUri = HttpContext.Current.Request.Url;
                string ShortenedUrlHost = CurrentUri.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
                
                Uri ShortenedUri;

                if (!String.IsNullOrWhiteSpace(link.ShortenedUrl) && 
                    link.IsVanityUrl &&
                    !Uri.TryCreate(link.ShortenedUrl, UriKind.Absolute, out ShortenedUri))
                {
                    //strip bad vanity chars
                    link.ShortenedUrl = System.Text.RegularExpressions.Regex.Replace(link.ShortenedUrl, "[\\~#%&*{}/:<>?|\"-]", "");
                    //prepend protocol & host
                    link.ShortenedUrl = ShortenedUrlHost + link.ShortenedUrl;
                }


                if(String.IsNullOrWhiteSpace(link.ShortenedUrl) || 
                    !Uri.TryCreate(link.ShortenedUrl, UriKind.RelativeOrAbsolute, out ShortenedUri) || 
                    (ShortenedUri.IsAbsoluteUri && !ShortenedUri.OriginalString.StartsWith(ShortenedUrlHost))){
                    //incoming shortened url is empty, not a valid uri, or refers to authority other than the current, 
                    //so, generate our own shortened url
                    link.ShortenedUrl = ShortenedUrlHost + UrlShortener.Generate();
                    link.IsVanityUrl = false;
                }

                //Verify this short URL is not in use
                while (db.Links.Any(l => l.ShortenedUrl == link.ShortenedUrl))
                {
                    if (!link.IsVanityUrl)
                    {
                        //if in use and not vanity, regenerate
                        link.ShortenedUrl = ShortenedUrlHost + UrlShortener.Generate();
                    }
                    else
                    {
                        //if in use and vanity, error
                        var message = "Shortened URL with this vanity path already exists.";
                        HttpError err = new HttpError(message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                }

                db.Links.Add(link);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, link);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = link.LinkId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}