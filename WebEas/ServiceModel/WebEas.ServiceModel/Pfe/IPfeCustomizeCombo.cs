using System.Collections.Generic;

namespace WebEas.ServiceModel
{
    public interface IPfeCustomizeCombo
    {
        /// <summary>
        /// Customizes <see cref="PfeComboAttribute"/>
        /// </summary>
        /// <param name="repository">Repository.</param>
        /// <param name="column">Názov property na ktorej combo definované.</param>
        /// <param name="kodPolozky">Kod položky na ktorej combo definované.</param>
        /// <param name="comboAttribute"><see cref="PfeComboAttribute"/></param>
        void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, Dictionary<string, string> requiredFields, ref PfeComboAttribute comboAttribute);
    }
}
