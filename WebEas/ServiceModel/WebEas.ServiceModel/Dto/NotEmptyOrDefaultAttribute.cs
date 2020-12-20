using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public class NotEmptyOrDefaultAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        public static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public override bool IsValid(object value)
        {
            return value != null && !value.Equals(GetDefault(value.GetType()));
        }
    }
}