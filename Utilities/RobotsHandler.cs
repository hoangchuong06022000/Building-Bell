using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreCaseStudy.Utilities
{
    public class RobotsHandler : IHttpHandler
    {
        private string defaultRobots = "User-agent: * \r\nDisallow";
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string robotsTxt = null;
            if ((Sitecore.Context.Site == null) || Sitecore.Context.Database == null)
            {
                robotsTxt = defaultRobots;
            }

            var homeItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);

            if (homeItem != null)
            {
                if (!string.IsNullOrEmpty(homeItem["Robots"]))
                {
                    robotsTxt = homeItem.Fields["Robots"].Value;
                }
            }

            context.Response.ContentType = "text-plain";
            context.Response.Write(robotsTxt);
        }
    }
}