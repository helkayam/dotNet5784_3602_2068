using System;
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
        string str = " ";
        foreach (PropertyInfo item in obj.GetType().GetProperties())
        {
            if (item.PropertyType.IsGenericTypeParameter&&item.PropertyType.GetGenericTypeDefinition() ==typeof(List<>))
            {

                str += "\n" + item.Name + ": "+ item.ToStringProperty();
              
                



            }
            else
            str += "\n" + item.Name + ": " + item.GetValue(obj, null);
        }

        return str;
    }


    public static string ToStringProperty<T>(this IEnumerable<T> obj)
    {
        string str = " ";
        foreach (T element in obj)
            str += element.ToStringProperty();

        return str;
    }


}
