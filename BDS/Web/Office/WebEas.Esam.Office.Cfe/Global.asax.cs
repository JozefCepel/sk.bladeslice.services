using ServiceStack.MiniProfiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebEas.Esam.Office.Cfe
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
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
#if DEBUG || DEVELOP || INT
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