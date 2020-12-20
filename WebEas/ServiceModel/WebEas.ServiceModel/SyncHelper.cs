using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
	public class SyncHelper
	{
		//internal purpose locking (lockTable access)
		object syncobj = new object();
		//collection of lock objects
		Dictionary<string, object> lockTable = new Dictionary<string, object>(50);

		/// <summary>
		/// Get synchronization object (to lock) for given code
		/// </summary>
		/// <param name="code">unique key (case insensitive)</param>
		public object GetSyncObject(string code)
		{
			string key = code.ToLowerInvariant();

			lock (syncobj)
			{
				object lockObj;
				if (!lockTable.TryGetValue(key, out lockObj))
				{
					lockObj = new object();
					lockTable.Add(key, lockObj);
				}
				return lockObj;
			}
		}
	}
}
