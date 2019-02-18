using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// Typ pristupu do aplikacie
    /// </summary>
    public enum WebEasAccessType : short
    {
        /// <summary>
        /// Pristup cez Service stack
        /// </summary>
        Json = 1,
        /// <summary>
        /// Pristup cez Wcf
        /// </summary>
        Soap = 2,
        /// <summary>
        /// Pristup cez IsoGw
        /// </summary>
        IsoGw = 3,
        /// <summary>
        /// Testovanie
        /// </summary>
        Debug = -1
    }
}