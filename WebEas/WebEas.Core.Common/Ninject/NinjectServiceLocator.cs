using System;
using System.Linq;
using Ninject;

namespace WebEas.Ninject
{
    /// <summary>
    /// Ninject Service Locator
    /// </summary>
    public static class NinjectServiceLocator
    {
        /// <summary>
        /// Gets or sets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public static IKernel Kernel { get; private set; }

        /// <summary>
        /// Sets the service locator.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public static void SetServiceLocator(IKernel kernel)
        {
            Kernel = kernel;
        }
    }
}