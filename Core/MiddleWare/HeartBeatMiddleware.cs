using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddleWare
{
    public class HeartBeatMiddleware
    {
        private readonly RequestDelegate next;

        public HeartBeatMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/Vehicles/getbyid"))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("403 hata kodu getbyid");
                return;
            }

            await next.Invoke(context);
        }
    }
}
