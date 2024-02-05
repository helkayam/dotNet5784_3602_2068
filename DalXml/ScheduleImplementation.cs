using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

public class ScheduleImplementation
{
    readonly string data_config = "data-config";

    public void UpdateStartDateProject(DateTime startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        root.Element("StartDateProject")?.SetValue((startDate));
        XMLTools.SaveListToXMLElement(root, data_config);

    }
    public DateTime? GetStartDateProject()
    {

        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        string? dt = root.Element("StartDateProject").Value;

        XMLTools.SaveListToXMLElement(root, data_config);
        if (dt != null)
            return DateTime.Parse(dt);
        else
            return null;
    }


}
