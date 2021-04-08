using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("STS_FIFOFull")]
    [DataContract]
    public class STS_FIFOFull : STS_FIFOFunction
    {
    }
}
