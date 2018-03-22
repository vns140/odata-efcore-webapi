using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ODataWebApp.OData
{
    public class ODataQueryStringFixer : IMiddleware
    {
        private static readonly Regex ReplaceToLowerRegex =
            new Regex(@"contains\((?<columnName>\w+),");

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var input = context.Request.QueryString.Value;
            var replacement = @"contains(tolower($1),";
            context.Request.QueryString =
                new QueryString(ReplaceToLowerRegex.Replace(input, replacement));

            return next(context);
        }
    }

    public static class ODataQueryStringFixerExtensions
    {
        public static IApplicationBuilder UseODataQueryStringFixer(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ODataQueryStringFixer>();
        }
    }
}