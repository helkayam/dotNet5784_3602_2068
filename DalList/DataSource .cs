using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Dal
{
    internal static class DataSource
    {
    
        internal static List<DO.Task?> Tasks { get; } = new();
        internal static List<DO.Dependency?> Dependencies { get; } = new();
        internal static List<DO.Worker?> Workers { get; } = new();
        internal static List<DO.User?> Users { get; } = new();



        /// <summary>
        /// A configuration class, which will generate automatic running numbers for us, for the fields that are defined as "running ID number".
        /// </summary>
        internal static class Config
        {

          
            //A constant numeric field that will receive an initial value for the Hertz number of Task
            internal const int startTaskId = 0;
            //A static numeric field that will receive as an initial value the previous fixed field of TaskId.
            private static int nextTaskId = startTaskId;
            //A property with a get method only that will advance the private field automatically, with a number greater than the previous one by 1
            internal static int NextTaskId { get => nextTaskId++; set => nextDependencyId = value; }

            //Likewise for DependencyId
            internal const int startDependencyId = 0;
            private static int nextDependencyId = startDependencyId;
            internal static int NextDependencyId { get => nextDependencyId++; set => nextDependencyId = value; }


           internal static  DateTime startdateProject;

            internal static DateTime CurrentDate;

            internal static DateTime EndDateProject;

            internal static string email { get => "d9349019@gmail.com"; }

            internal static string password { get => "dotNet20683602"; }

            public static void ResetNextDependencyId()
            {
                NextDependencyId = 0;
            }

            public static void ResetNextTaskId()
            {
                NextTaskId = 0;
            }


        }

      
    }
}
