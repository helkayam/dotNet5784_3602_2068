using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO;

static internal class Tools
{

    public static string ToStringProperty<T>(this T obj)
    {
        StringBuilder sb = new StringBuilder();

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo prop in properties)
        {
            sb.Append(prop.Name);
            sb.Append(": ");

            object value = prop.GetValue(obj)!;

            if (value is IEnumerable && !(value is string))
            {
                IEnumerable enumerable = (IEnumerable)value;
                //if (value is ICollection collection && collection.Count == 0)
                //    break;
                    sb.Append("[");
                foreach (object item in enumerable)
                {
                    sb.Append(item.ToString());
                    sb.Append(", ");
                }
                if (sb[sb.Length - 2] == ',')
                {
                    sb.Remove(sb.Length - 2, 2); // Remove trailing comma and space
                }
                sb.Append("]");
               
            }
            else
            {
                sb.Append(value?.ToString() ?? "null");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

}



