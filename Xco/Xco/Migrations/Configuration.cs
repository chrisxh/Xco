namespace Xco.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Xco.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Xco.Models.XcoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Xco.Models.XcoContext context)
        {
            //context.Links.AddOrUpdate(
            //        l => l.ShortenedUrl,
            //        new Link
            //        {
            //            OriginalUrl = "http://www.asp.net/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api",
            //            ShortenedUrl = "http://x.co/wapi",
            //            CreatedOn = DateTime.Now,
            //            CreatedByEmail = "chris.hoehn@gmail.com",
            //            IsVanityUrl = true
            //        },
            //        new Link
            //        {
            //            OriginalUrl = "http://www.asp.net/mvc/tutorials/mvc-music-store/mvc-music-store-part-8",
            //            ShortenedUrl = "http://x.co/zXaaB",
            //            CreatedOn = DateTime.Now.AddDays(-13),
            //            CreatedByEmail = "chris.hoehn@gmail.com",
            //            IsVanityUrl = false
            //        },
            //        new Link
            //        {
            //            OriginalUrl = "https://docs.google.com/a/mnaspa.org/file/d/0B2-COLG99_R3WVAzUkwxcXVEcWc/edit",
            //            ShortenedUrl = "http://x.co/Fall12Letter",
            //            CreatedOn = DateTime.Now.AddDays(-32),
            //            CreatedByEmail = "chris.hoehn@gmail.com",
            //            IsVanityUrl = true
            //        },
            //        new Link
            //        {
            //            OriginalUrl = "http://odetocode.com/Blogs/scott/archive/2012/08/31/seeding-membership-amp-roles-in-asp-net-mvc-4.aspx",
            //            ShortenedUrl = "http://x.co/zCg83",
            //            CreatedOn = DateTime.Now,
            //            CreatedByEmail = "chris.hoehn@gmail.com",
            //            IsVanityUrl = false
            //        },
            //        new Link
            //        {
            //            OriginalUrl = "https://docs.google.com/a/mnaspa.org/file/d/0B2-COLG99_R3WVAzUkwxcXVEcWc/edit",
            //            ShortenedUrl = "http://x.co/xpired",
            //            CreatedOn = DateTime.Now.AddDays(-32),
            //            CreatedByEmail = "chris.hoehn@gmail.com",
            //            IsVanityUrl = true,
            //            ActivationDate = DateTime.Now.AddDays(-32),
            //            DeactivationDate = DateTime.Now.AddDays(-3)
            //        });
        }
    }
}
