using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public interface ICasovaPlatnost
    {
        [IgnoreDataMember()]
        DateTime? PlatnostOd { get; set; }

        [IgnoreDataMember()]
        DateTime? PlatnostDo { get; set; }
    } 
}