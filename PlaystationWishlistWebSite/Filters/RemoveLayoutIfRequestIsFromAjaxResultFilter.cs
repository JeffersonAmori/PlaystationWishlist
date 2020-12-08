using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlaystationWishlistWebSite.Filters
{
    public class RemoveLayoutIfRequestIsFromAjaxResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //if (!(context.Result is ViewResult viewResult)) 
            //    return;

            //if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    viewResult.ViewData["Layout"] = "";
            //}
            //else
            //{
            //    viewResult.ViewData["Layout"] = "_Layout";
            //}
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //if (!(context.Result is ViewResult viewResult))
            //    return;

            //if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    viewResult.ViewData["Layout"] = "";
            //}
            //else
            //{
            //    viewResult.ViewData["Layout"] = "_Layout";
            //}
        }
    }
}
