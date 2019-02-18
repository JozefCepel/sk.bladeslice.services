using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using ServiceStack.Logging;
using WebEas.Auth;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseServiceModel : IServiceModel
    {
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <value>The role list.</value>
        public abstract List<Role> RoleList { get; }

        /// <summary>
        /// Gets the XSD types.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, Type> GetXsdTypes()
        {
            return this.AddXsdTypes(new Dictionary<string, Type>());
        }

        /// <summary>
        /// Adds the XSD types.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public virtual Dictionary<string, Type> AddXsdTypes(Dictionary<string, Type> collection)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int count = 0;
            foreach (Type t in this.GetType().Assembly.GetTypes())
            {
                if (!String.IsNullOrEmpty(t.Namespace) && t.Namespace.Contains(".Xsd.") && t.BaseType != null && t.BaseType.Name == "RequestBase")
                {
                    XmlRootAttribute attr = t.GetCustomAttributes<XmlRootAttribute>().FirstOrDefault();
                    string name;
                    if (attr != null)
                    {
                        name = string.Format("{{{0}}}{1}", attr.Namespace, t.Name);                       
                    }
                    else
                    {
                        name = t.Name;
                    }
                    if (collection.ContainsKey(name))
                    { 
                        log.Error(string.Format("Xsd type already exists - duplicity {0} in dictionary {1} next {2}", name, collection[name], t));                        
                    }
                    else
                    {
                        collection.Add(name, t);
                        count++;
                    }
                }
            }
       
            log.Debug(string.Format("Founded {0} types for XSD in {1}", count, this.GetType()));
            return collection;
        }
    }
}