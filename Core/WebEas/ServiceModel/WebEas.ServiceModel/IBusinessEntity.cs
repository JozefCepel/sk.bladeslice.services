using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public interface IBusinessEntity
    {
        
    }

    public interface IBaseEvidencia
    {
        string Riesitel { get; set; }
        bool ZbernySpis { get; set; }
    }
}