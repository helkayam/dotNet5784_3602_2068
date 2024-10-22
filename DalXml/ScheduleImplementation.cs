﻿using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

public class ScheduleImplementation:ISchedule 
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

       // XMLTools.SaveListToXMLElement(root, data_config);
        if (dt != "")
            return DateTime.Parse(dt);
        else
            return null;
    }

    public void UpdateCurrentDate(DateTime currentdt)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        root.Element("CurrentDate")?.SetValue((currentdt));
        XMLTools.SaveListToXMLElement(root, data_config);

    }

    public void UpdateEndDateProject(DateTime currentdt)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        root.Element("EndDateProject")?.SetValue((currentdt));
        XMLTools.SaveListToXMLElement(root, data_config);

    }
    public DateTime? GetCurrentDate()
    {

        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        string? dt = root.Element("CurrentDate").Value;

        XMLTools.SaveListToXMLElement(root, data_config);
        if (dt != "")
            return DateTime.Parse(dt);
        else
            return null;
    }
    public DateTime? getEndDateProject()
    {

        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        string? dt = root.Element("EndDateProject").Value;

        XMLTools.SaveListToXMLElement(root, data_config);
        if (dt != "")
            return DateTime.Parse(dt);
        else
            return null;
    }

    public void ResetEndStartDateProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        root.Element("StartDateProject")?.SetValue((""));
        root.Element("EndDateProject")?.SetValue((""));
        XMLTools.SaveListToXMLElement(root, data_config);

    }

    

}
