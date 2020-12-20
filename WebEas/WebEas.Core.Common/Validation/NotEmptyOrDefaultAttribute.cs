using System;
using System.Linq;

namespace WebEas
{
    public class NotEmptyOrDefaultAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        public static object GetDefault<T>(T type) where T : Type
        {
            return default(T);
        }

        public override bool IsValid(object value)
        {
            return value != null && value != GetDefault(value.GetType());
        }
    }
}