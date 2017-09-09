using ApiView.Code;
using ApiView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ApiView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApiDetails(string id)
        {
            var model = new ApiDetailsModel();
            model.api = ApiHelper.AllApis.Where(c => c.ID == id).SingleOrDefault();
            model.doc = ApiHelper.GetOpenApiAttribute(model.api.Method);
            return PartialView("_PartialApiDetails", model);
        }

        /// <summary>
        /// search
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult Search(string keyword)
        {
            ViewBag.Keyword = keyword;
            List<Api> openapis = new List<Api>();
            ApiHelper.AllApis.ForEach(c =>
            {
                var o = ApiHelper.GetOpenApiAttribute(c.Method);
                if ((ContainKeywords(o.MethodDescription, keyword))
                    || (ContainKeywords(o.MethodName, keyword))
                    || (ContainKeywords(o.ParamDescription, keyword))
                    || (ContainKeywords(o.ResultDescription, keyword))
                    || (ContainKeywords(o.Note, keyword))
                    || (ContainKeywords(o.Author, keyword))
                    || (ContainKeywords(c.RelateUrl(), keyword)))
                {
                    openapis.Add(c);
                }
            });
            return PartialView("_PartialApiSearch", openapis);
        }

        public ActionResult Help()
        {
            return PartialView("_PartialApiHelp");
        }

        private bool ContainKeywords(string content, string keywords)
        {
            var ks = keywords.Trim().Split(' ');
            foreach (var k in ks)
            {
                if (content != null && content.ToLower().Contains(k.ToLower()))
                    continue;
                else
                    return false;
            }
            return true;
        }

    }
}
