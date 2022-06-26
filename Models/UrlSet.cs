using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SitecoreCaseStudy.Models
{
    [XmlRoot("urlset")]
    public class UrlSet
    {
        [XmlElement("url")]
        public List<Url> UrlColection { get; set; }
    }
    public class Url
    {
        [XmlElement("location")]
        public string Location { get; set; }

        [XmlElement("itemname")]
        public string ItemName { get; set; }

        [XmlElement("itempath")]
        public string ItemPath { get; set; }

        [XmlElement("itemid")]
        public string ItemID { get; set; }

        [XmlElement("lastmod")]
        public string LastMod { get; set; }
    }
}