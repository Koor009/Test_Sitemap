using System;
using System.Web.Mvc;

namespace Test_Sitemap.Filters
{
    public class FilesError : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is Exception)
            {
                exceptionContext.Result = new RedirectResult("/Home/Error");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}