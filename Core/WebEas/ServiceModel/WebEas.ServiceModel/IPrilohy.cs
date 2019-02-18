using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public interface IPrilohy<T>
    {
        [Ignore]
        List<T> Prilohy { get; set; }
    }
}