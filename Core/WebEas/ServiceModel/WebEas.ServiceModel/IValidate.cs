using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public interface IValidate
    {
        /// <summary>
        /// Validovanie dat pred insertom a updatom
        /// </summary>
        void Validate();
    }
}