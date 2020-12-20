using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public interface IBaseEntity : IChangeIdentifier
    {
        /// <summary>
        /// Gets or sets the vytvoril.
        /// </summary>
        /// <value>The vytvoril.</value>
        Guid? Vytvoril { get; set; }

        /// <summary>
        /// Gets or sets the datum vytvorenia.
        /// </summary>
        /// <value>The datum vytvorenia.</value>        
        DateTime DatumVytvorenia { get; set; }

        /// <summary>
        /// Gets or sets the datum zmeny.
        /// </summary>
        /// <value>The datum zmeny.</value>        
        DateTime DatumZmeny { get; set; }

        /// <summary>
        /// Gets or sets the zmenil.
        /// </summary>
        /// <value>The zmenil.</value>
        Guid? Zmenil { get; set; }

        /// <summary>
        /// Gets or sets the poznamka.
        /// </summary>
        /// <value>The poznamka.</value>        
        string Poznamka { get; set; }

        /// <summary>
        /// Gets the change identifier.
        /// </summary>
        /// <value>The change identifier.</value>
        long ChangeIdentifier { get; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <returns></returns>
        object GetId();

        /// <summary>
        /// Optimalizacia sql update - updatujeme len zmenene property
        /// </summary>
        object DefaultRecord { get; set; }
    }
}