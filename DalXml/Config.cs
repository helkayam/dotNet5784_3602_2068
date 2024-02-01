using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
/// <summary>
/// This is a configuration file in which we can save general data that is not part of the data lists themselves. For example: 
/// a data that stores the last running number for a certain entity. that will be saved from run to run,
/// and in the next run we can start from the last given run number.
///The data we will save in this file generally corresponds to the fields we defined in the Config class found in the DalList project.
/// </summary>
/// 
internal static class Config
{
    /// <summary>
    /// This is a private field that will hold the name of the general data configuration file
    /// </summary>
    static string s_data_config_xml = "data-config";

    /// <summary>
    /// This is a field that gets us the next running number of Task by using the XmlTools class
    /// </summary>
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    /// <summary>
    /// This is a field that gets us the next running number of Dependency by using the XmlTools class
    /// </summary>
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }


    internal static DateTime StartDateProject { get => XMLTools.ToDateTimeNullable(""); } 

}
