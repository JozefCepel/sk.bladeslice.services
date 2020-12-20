using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceInterface
{
    public interface ISave<T>
    {
        T Save(T t);
    }

    public interface ISave<T, R>
    {
        R Save(T t);
    }
}
