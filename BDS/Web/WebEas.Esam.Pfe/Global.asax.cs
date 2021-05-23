using ServiceStack.MiniProfiler;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebEas.Esam.Pfe
{
    /// <summary>
    /// 
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
            
            var host = string.Empty;
#if !DEBUG
            host = "localhost";
#endif

            var bootPaths = new List<string>
            {
                $"http://{host}/esam/api/office/cfe/v1/app-status",
                $"http://{host}/esam/api/office/crm/v1/app-status",
                $"http://{host}/esam/api/office/dap/v1/app-status",
                $"http://{host}/esam/api/office/dms/v1/app-status",
                $"http://{host}/esam/api/office/fin/v1/app-status",
                $"http://{host}/esam/api/office/osa/v1/app-status",
                $"http://{host}/esam/api/office/reg/v1/app-status",
                $"http://{host}/esam/api/office/rzp/v1/app-status",
                $"http://{host}/esam/api/office/uct/v1/app-status",
                $"http://{host}/esam/api/office/vyk/v1/app-status",
                $"http://{host}/esam/api/reports/formats"
            };

            if (!string.IsNullOrEmpty(host))
            {
                foreach (var path in bootPaths)
                {
                    Task.Run(() =>
                    {
                        using var client = new HttpClient();
                        client.GetAsync(path).Wait();
                    });
                }
            }
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
#if DEBUG || DEVELOP || INT || ITP
            Profiler.Start();
#else
            if (this.Request.IsLocal)
            {
                Profiler.Start();
            }
#endif
        }

        /// <summary>
        /// Handles the EndRequest event of the Application control.
        /// </summary>
        /// <param name="src">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void Application_EndRequest(object src, EventArgs e)
        {
            Profiler.Stop();
        }
    }
}