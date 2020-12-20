using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
    public enum SingleModeActionEnum
    {
        None = 0,

        // Hlavná akcia
        Main,

        // Podradená/vnorená akcia
        Nested
    }
}
