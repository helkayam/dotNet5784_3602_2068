using System.Collections.Specialized;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Dal;
using DalApi;
using DO;


namespace DalTest

{

    internal class Program
    {
        static readonly IDal s_dal = new Dal.DalList(); //stage 2

        private static readonly Random s_rand = new();
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dal);
                MainPage();
                int choice = int.Parse(Console.ReadLine());
                while (choice != 0)
                {
                    switch (choice)
                    {
                        case 1: WorkerPage(); break; 
                        case 2: TaskPage(); break; 
                        case 3: DependencyPage();break; 
                        default:break;
                    }
                    MainPage();
                    choice = int.Parse(Console.ReadLine());
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// this method print the Main Page 
        /// </summary>
        static void MainPage()
        {

            Console.WriteLine("Select an entity you want to check");
            Console.WriteLine("0.Exit from main menu");
            Console.WriteLine("1.Worker entity");
            Console.WriteLine("2.Task entity");
            Console.WriteLine("3.dependency entity");
        }

        /// <summary>
        /// this method print the worker page with all the action we can do (like add a task,update,delete...) and ask from the user wich action does he want to do 
        /// </summary>
        private static void WorkerPage()
        {
            try
            {
                Console.WriteLine("Select the method you want to perform");
                Console.WriteLine("1.Exit main menu");
                Console.WriteLine("2.Adding a new worker to the list");
                Console.WriteLine("3.worker display by ID");
                Console.WriteLine("4.View a list of all workers");
                Console.WriteLine("5.Update existing worker data");
                Console.WriteLine("6.Deletion of an existing worker from the list");
                int choice = int.Parse(Console.ReadLine());
                if (choice != 1)
                {
                    switch (choice)
                    {
                        case 2: createW(); break;
                        case 3: readW(); break;
                        case 4: readAllW(); break;
                        case 5: updateW(); break;
                        case 6: deleteW(); break;
                        default: break;

                    }
                }
            }
            catch(Exception ex)
            { Console.WriteLine(ex.ToString); }

        }

        /// <summary>
        /// this method print the task page with all the action we can do (like add a task,update,delete...) and ask from the user wich action does he want to do 
        /// </summary>
        private static void TaskPage()
        {
           
                Console.WriteLine("Select the method you want to perform");
                Console.WriteLine("1.Exiting the main menu");
                Console.WriteLine("2.Adding a new task to the list");
                Console.WriteLine("3.Task display by ID");
                Console.WriteLine("4.Display list of all tasks");
                Console.WriteLine("5.Updating existing task data");
                Console.WriteLine("6.Deleting an existing task from the list");
                int choice = int.Parse(Console.ReadLine());
                if (choice != 1)
                {
                    switch (choice)
                    {
                        case 2: createT(); break;
                        case 3: readT(); break;
                        case 4: readAllT(); break;
                        case 5: updateT(); break;
                        case 6: deleteT(); break;
                        default: break;

                    }
                }
            
            

        }


        /// <summary>
        /// this method print the dependency page with all the action we can do (like add a task,update,delete...) and ask from the user wich action does he want to do 
        /// </summary>
        private static void DependencyPage()
        {
            Console.WriteLine("Select the method you want to perform");
            Console.WriteLine("1.Exiting the main menu");
            Console.WriteLine("2.Adding a new dependency to the list");
            Console.WriteLine("3.Dependency display by ID");
            Console.WriteLine("4.Display the list of all dependencies");
            Console.WriteLine("5.Updating existing dependency data");
            Console.WriteLine("6.Deleting an existing dependency from the list");
            int choice = int.Parse(Console.ReadLine());
            if (choice != 1)
            {
                switch (choice)
                {
                    case 2: createD(); break;
                    case 3:readD();break;
                    case 4:readAllD();break;
                    case 5:updateD(); break;    
                    case 6:deleteD();break;
                    default: break;

                }
            }
        }


        /// <summary>
        /// this method creates a new Worker
        /// </summary>
        private static void createW()
        {
            Console.WriteLine("Enter ID, experience, name, phone number and hourly payment");
            int id = int.Parse(Console.ReadLine());
            WorkerExperience we = (WorkerExperience)int.Parse(Console.ReadLine());
            string name = Console.ReadLine();
            string phonenumber = Console.ReadLine();
            double cost = double.Parse(Console.ReadLine());
            Worker w = new Worker(id, we, name, phonenumber, cost);
            try
            {
                s_dalWorker.Create(w);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// this method creates a new task 
        /// </summary>
        private static void createT()
        {
            Console.WriteLine("Enter alias of the task, scheduled start date of the task, deadline date of the task, the complexity of the task and description of the task");
            string alias = Console.ReadLine();
            DateTime ScheduledDate = DateTime.Now;

            DateTime Deadline = DateTime.Now + TimeSpan.FromDays(s_rand.Next(5, 20));
            WorkerExperience we = (WorkerExperience)int.Parse(Console.ReadLine());
            string description = Console.ReadLine();
            DO.Task t = new DO.Task(alias, we,description, ScheduledDate, Deadline);
            try
            {
                s_dalTask.Create(t);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// this method creates a new dependency
        /// </summary>
        private static void createD()
        {
            Console.WriteLine("Enter pending task ID number and previous task ID number");
            int dependenTask = int.Parse(Console.ReadLine());
            int dependOnTask = int.Parse(Console.ReadLine());
            Dependency d = new Dependency(dependenTask, dependOnTask);
            try
            {
                s_dalDependency.Create(d);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// this method gets from the user id of a worker and print the worker data
        /// </summary>
        private static void readW()
        {
            Console.WriteLine("Enter ID of the worker");
            int id = int.Parse(Console.ReadLine());
            Worker? w = s_dalWorker.Read(id);
            if (w != null)
            {
                Console.WriteLine(w);
            }

        }

        /// <summary>
        /// this method gets from the user id of a task and print the task data
        /// </summary>
        private static void readT()
        {
            Console.WriteLine("Enter ID of the task");
            int id = int.Parse(Console.ReadLine());
            DO.Task? t= s_dalTask.Read(id);
            if (t != null)
            {
                Console.WriteLine(t);
            }

        }

        /// <summary>
        /// this method gets from the user id of a dependency and print the dependency data
        /// </summary>
        private static void readD()
        {
            Console.WriteLine("Enter ID of the dependency");
            int id = int.Parse(Console.ReadLine());
            Dependency d = s_dalDependency.Read(id);
            if (d != null)
            {
                Console.WriteLine(d);
            }
        }


        /// <summary>
        /// this method prints all the workers data
        /// </summary>
        private static void readAllW()
        {
            List<Worker> w = s_dalWorker.ReadAll();

            foreach(Worker x in w)
            {
                Console.WriteLine(x);
                Console.WriteLine("\n");
            }
        }

        /// <summary>
        /// this method prints all the tasks data
        /// </summary>
        private static void readAllT()
        {
            List<DO.Task> t = s_dalTask.ReadAll();
            foreach(DO.Task x in t)
            {
                Console.WriteLine(x);
                Console.WriteLine("\n");    
            }

        }

        /// <summary>
        /// this method prints all the dependencies data
        /// </summary>
        private static void readAllD()
        {
            List<Dependency> d = s_dalDependency.ReadAll();
            foreach(Dependency x in d)
            {
                Console.WriteLine(x);
                Console.WriteLine("\n");    
            }
        }


        /// <summary>
        /// this method gets from the user id of a worker and update data of that worker 
        /// </summary>
        /// <exception cref="Exception">throw exception if there is no worker with that id>
        private static void updateW()
        {
            try
            {
                Console.WriteLine("Enter the ID number of the worker you want to update");
                int id = int.Parse(Console.ReadLine());
                Worker w = s_dalWorker.Read(id);
                if (w != null)
                    Console.WriteLine(w);
                else
                    throw new Exception($"Worker with ID={id} does not exist");

                Console.WriteLine("Enter experience, name, phone number and hourly payment of the worker");
                WorkerExperience we = (WorkerExperience)int.Parse(Console.ReadLine());
                string name = Console.ReadLine();
                string phonenumber = Console.ReadLine();
                double cost = double.Parse(Console.ReadLine());
                Worker worker = new Worker(id, we, name, phonenumber, cost);
                s_dalWorker.Update(worker);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// this method gets from the user id of a task and update data of that task 
        /// </summary>
        /// <exception cref="Exception">throw exception if there is no task with that id
        private static void updateT()
        {
            try
            {
                Console.WriteLine("Enter the ID number of the task you want to update");
                int id = int.Parse(Console.ReadLine());
                DO.Task t = s_dalTask.Read(id);
                if (t != null)
                    Console.WriteLine(t);
                else
                    throw new Exception($"Task with ID={id} does not exist");

                Console.WriteLine("Enter alias of the task, scheduled start date of the task, deadline of the task, the complexity of the task and description of the task,");
                string alias = Console.ReadLine();
                DateTime ScheduledDate = DateTime.Now;
                DateTime Deadline = DateTime.Now + TimeSpan.FromDays(s_rand.Next(5, 20));
                WorkerExperience we = (WorkerExperience)int.Parse(Console.ReadLine());
                string description = Console.ReadLine();
                DO.Task task = new DO.Task(alias, we, description, ScheduledDate, Deadline);
                s_dalTask.Update(task);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// this method gets from the user id of a dependency and update that dependency
        /// </summary>
        /// <exception cref="Exception">throw exception if there is no dependency with that id>
        private static void updateD()
        {
            try
            {
                Console.WriteLine("Enter the ID number of the dependency you want to update");
                int id = int.Parse(Console.ReadLine());
                Dependency d = s_dalDependency.Read(id);
                if (d != null)
                    Console.WriteLine(d);
                else
                    throw new Exception($"Dependency with ID={id} does not exist");

                Console.WriteLine("Enter pending task ID number and previous task ID number");
                int dependentTask = int.Parse(Console.ReadLine());
                int dependOnTask = int.Parse(Console.ReadLine());
                Dependency dependency = new Dependency(dependentTask, dependOnTask);
                s_dalDependency.Update(dependency);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// this method gets from the user an id of a worker and delete this worker 
        /// </summary>
        private static void deleteW()
        {
            try
            {
                Console.WriteLine("Enter the ID of the worker you want to delete");
                int id = int.Parse(Console.ReadLine());
                s_dalWorker.Delete(id);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// this method gets from the user an id of a task and delete this task
        /// </summary>
        private static void deleteT()
        {
            try
            {
                Console.WriteLine("Enter the ID of the task you want to delete");
                int id = int.Parse(Console.ReadLine());
                s_dalTask.Delete(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// this method gets from the user an id of a dependency and delete it
        /// </summary>
        private static void deleteD()
        {
            try
            {
                Console.WriteLine("Enter the ID of the dependency you want to delete");
                int id = int.Parse(Console.ReadLine());
                s_dalDependency.Delete(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
