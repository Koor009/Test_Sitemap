using System;
using System.Web.Mvc;

namespace Test_Sitemap.Filters
{
    internal sealed class FilesError : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Extensible method of ExceptionContext.
        /// </summary>
        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is System.Net.WebException)
            {
                exceptionContext.Result = new RedirectResult("/Home/Error403");
                exceptionContext.ExceptionHandled = true;
            }
            else if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is Exception)
            {
                exceptionContext.Result = new RedirectResult("/Home/Error");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}