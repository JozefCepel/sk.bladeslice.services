using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// Prístup do dcom ako
    /// </summary>
    public enum WebEasUserType : short
    {
        /// <summary>
        /// Občan - Fyzická osoba
        /// </summary>
        UserFo = 1,
        /// <summary>
        /// Občan -  Právnická osoba
        /// </summary>
        UserPo = 2,
        /// <summary>
        /// Úradník obce
        /// </summary>
        Clerk = 10,
        /// <summary>
        /// Technický účet
        /// </summary>
        TechUser = -2,
        /// <summary>
        /// Anonymný používateľ
        /// </summary>
        Anonym = -1
    }
}