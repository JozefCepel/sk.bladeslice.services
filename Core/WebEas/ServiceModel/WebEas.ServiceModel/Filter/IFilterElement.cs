using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
	public interface IFilterElement
	{

		string ConnOperator { get; set; }
        //string Key { get; set; }
        IFilterElement Clone();
		Filter ToFilter();
		void ToSqlString(StringBuilder where);
        void AddParameters(Dictionary<string, object> parameters);

	}
}
