using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infra.SqlServerWithEF.Exceptions;

[AttributeUsage(AttributeTargets.All)]
internal class DbChangeExceptionAttribute : Attribute, IExceptionFilter {
    public void OnException(ExceptionContext context) {
        if(context.Exception == null) {
            return;
        }
        context.ExceptionHandled = true;
        context.Result = new JsonResult(new {
            code = "OnDBSaveChanges" ,
            where = context.ActionDescriptor.DisplayName ,
            message = context.Exception.Message
        });
    }
}
