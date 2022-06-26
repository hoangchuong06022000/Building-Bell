using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreCaseStudy.Utilities
{
    public class RobotsHandler : IHttpHandler
    {
        private string defaultRobots = "User-agent: * \r\n Disallow";
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string robotsTxt = defaultRobots;
            if ((Sitecore.Context.Site == null) || Sitecore.Context.Database == null)
            {
                robotsTxt = defaultRobots;
            }

            var homeItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
            var robotsItem = homeItem.Axes.GetDescendant("Robots");

            if (robotsItem != null)
            {
                if (!string.IsNullOrEmpty(robotsItem["Robots"]))
                {
                    robotsTxt = robotsItem.Fields["Robots"].Value;
                }
            }

            context.Response.ContentType = "text-plain";
            context.Response.Write(robotsTxt);
        }
    }
}