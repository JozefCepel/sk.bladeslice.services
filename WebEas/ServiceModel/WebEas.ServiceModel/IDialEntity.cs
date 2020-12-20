using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public interface IDialEntity<T>
    {
        T ItemCode { get; set; }

        string ItemName { get; set; }
    }

    public interface IDialEntity : IDialEntity<string>
    {
    }
}