﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
    public interface IOrsPravo
    {
        byte OrsPravo { get; set; }

        void ApplyOrsPravoToAccesFlags();
    }
}
