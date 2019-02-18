using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Urcuje kod ciselnika - zoznam v navrhove dokumenty/Stav_Mapovanie_formulare_sluzby.xlsx
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]    
    public class CodeListCodeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListCodeAttribute" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public CodeListCodeAttribute(string code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }
    }
}