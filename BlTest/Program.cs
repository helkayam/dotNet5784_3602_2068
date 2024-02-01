using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using BL;
using BlApi;
using BO;
using DalApi;

//using DO;

namespace BlTest

{
    internal class Program
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static void Main(string[] args)
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
            {
               
               
                s_bl.Worker.deleteAll();
                s_bl.Task.deleteAll();
                DalTest.Initialization.Do();
            }



            try
            {
                MainPage();
                int choice = int.Parse(Console.ReadLine());
                while (choice != 0)
                {
                    switch (choice)
                    {
                        case 1: WorkerPage(); break;
                        case 2: TaskPage(); break;
                        default: break;
                    }
                    MainPage();
                    choice = int.Parse(Console.ReadLine());
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());
            }

        }
        static void MainPage()
        {

            Console.WriteLine("Select an entity you want to check");
            Console.WriteLine("0.Exit from main menu");
            Console.WriteLine("1.Worker entity");
            Console.WriteLine("2.Task entity");
        }

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
            catch (Exception ex)
            { Console.WriteLine(ex.ToString); }

        }

        private static void TaskPage()
        {

            Console.WriteLine("Select the method you want to perform");
            Console.WriteLine("1.Exiting the main menu");
            Console.WriteLine("2.Adding a new task to the list");//Hen
            Console.WriteLine("3.Task display by ID");//Lea Done
            Console.WriteLine("4.Display list of all tasks");//Hen
            //Console.WriteLine("5.Display list of all tasks in group by status");//Lea Done
            Console.WriteLine("6.Updating existing task data");//Lea Done
            Console.WriteLine("7.Updating start date of existing task");//Lea Done
            Console.WriteLine("8.Deleting an existing task from the list");//Hen

            int choice = int.Parse(Console.ReadLine());
            if (choice != 1)
            {
                switch (choice)
                {
                    case 2: createT(); break;
                    case 3: readT(); break;
                    case 4: readAllT(); break;
                    //case 5: printGroups(); break;
                    case 6: updateT(); break;
                    case 7: AddUpdDateDate();break;
                    case 8: deleteT(); break;
                    default: break;

                }
            }



        }

        private static void createW()
        {


            Console.WriteLine("Enter ID, name,level, phone number ,hourly payment,id of task,alias of task ");
            try
            {
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");

                string name = Console.ReadLine();
                BO.WorkerExperience we = (BO.WorkerExperience)int.Parse(Console.ReadLine());
                string phonenumber = Console.ReadLine();


                double cost;
                string SCost = Console.ReadLine();
                res = double.TryParse(SCost, out cost);
                if (res == false)
                    throw new Exception("Cant convert hourly payment of worker from string to double");


                int IdOfTask;
                string sIdTask = (Console.ReadLine());
                res = int.TryParse(sIdTask, out IdOfTask);
                if (res == false)
                    throw new Exception("Cant convert ID Task of worker from string to int");

                string aliasOfTask = Console.ReadLine();

                BO.TaskInWorker TaskOfWorker = new BO.TaskInWorker { Id = IdOfTask, Alias = aliasOfTask };
                BO.Worker myNewWorker = new BO.Worker { Name = name, Id = id, Level = we, PhoneNumber = phonenumber, Cost = cost, Task = TaskOfWorker };


                s_bl.Worker.AddWorker(myNewWorker);
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine($"BlAlreadyExistsException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                Console.WriteLine($"BlInvalidGivenValueException: {ex.Message} ");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }



        private static void createT()
        {
            try
            {
                Console.WriteLine("Enter description, alias, erasable of the task, ScheduledDate,Deadline, Remarks, name of worker ,Id of Worker, Complexity of the task ");
                string description = Console.ReadLine();
                string alias = Console.ReadLine();

                DateTime creatingDate=DateTime.Now;
              

                bool erasable = bool.Parse(Console.ReadLine());


                DateTime ScheduledDate;
                string sSchedule = (Console.ReadLine());
               bool  res = DateTime.TryParse(sSchedule, out ScheduledDate);
                if (res == false)
                    throw new Exception("Cant convert ScheduledDate Date of Task from string to DateTime");

                DateTime Deadline;
                string sDeadline = (Console.ReadLine());
                res = DateTime.TryParse(sDeadline, out Deadline);
                if (res == false)
                    throw new Exception("Cant convert Deadline Date of Task from string to DateTime");

                string Remarks = Console.ReadLine();
                string NameOfWorker = Console.ReadLine();

                int IdOfWorker;
                string sIdWorker = Console.ReadLine();
                res = int.TryParse(sIdWorker, out IdOfWorker);
                if (res == false)
                    throw new Exception("Cant convert ID of worker on Task from string to int");

                BO.WorkerInTask myWorker = new BO.WorkerInTask { Id = IdOfWorker, Name = NameOfWorker };

                int complexity;
                string sComplexity = Console.ReadLine();
                res = int.TryParse(sComplexity, out complexity);
                if (res == false)
                    throw new Exception("Cant convert Complexity from string to int");
                BO.WorkerExperience TaskComplexity = (BO.WorkerExperience)complexity;
                Console.WriteLine("Enter number of dependent tasks");
                int numOfDep;
                string SNumOfDep = Console.ReadLine();
                res = int.TryParse(SNumOfDep, out numOfDep);
                if (res == false)
                    throw new Exception("Cant convert number of dependent tasks from string to int");

                Console.WriteLine("Enter ID of dependent tasks");
                List<TaskInList> depTasks = new List<TaskInList>();
                for (int i = 0; i < numOfDep; i++)
                {
                    int idOfTask;
                    string sIdTask = Console.ReadLine();
                    res = int.TryParse(sIdTask, out idOfTask);
                    if (res == false)
                        throw new Exception("Cant convert ID of dependent Task from string to int");
                    BO.TaskInList taskInList = new BO.TaskInList { Id = idOfTask };
                    depTasks.Add(taskInList);
                }

                BO.Task newTask = new BO.Task
                {
                    Description = description,
                    Alias = alias,
                    CreatedAtDate = creatingDate,
                    Eraseable = erasable,
                    Dependencies = depTasks,
                    ScheduledDate = ScheduledDate,
                    Deadline = Deadline,
                    Remarks = Remarks,
                    Worker = myWorker,
                    Complexity = TaskComplexity
                };

                

                s_bl.Task.AddTask(newTask);
            }


              catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine($"BlAlreadyExistsException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                Console.WriteLine($"BlInvalidGivenValueException: {ex.Message} ");

            }
            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException:{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// this method gets from the user id of a worker and print the worker data
        /// </summary>
        private static void readW()
        {
            try
            {
                Console.WriteLine("Enter ID of the worker");
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");
                Worker? w = s_bl.Worker.ReadWorker(id);

                if (w != null)
                {
                    Console.WriteLine(w);
                }
            }
            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException}");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        /// <summary>
        /// this method gets from the user id of a task and print the task data
        /// </summary>
        private static void readT()
        {
            try
            {
                Console.WriteLine("Enter ID of the task");
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");

                BO.Task? t = s_bl.Task.ReadTask(id);
                
                if (t != null)
                {
                    Console.WriteLine(t);
                }
            }
            catch(BO.BlDoesNotExistException ex )
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static int FilterMenuW()
        {
            Console.WriteLine("Select the filter you want to operate");
            Console.WriteLine("0.filter by Level");
            Console.WriteLine("1.filter by activity of workers");
            Console.WriteLine("2.filter by erasable");
            Console.WriteLine("3.filter by workers Without task");
            Console.WriteLine("4.None");

            int choice = int.Parse(Console.ReadLine());
            return choice;
        }


        private static int FilterMenuT()
        {
            Console.WriteLine("Select the filter you want to operate");
            Console.WriteLine("0.filter by Complexity");
            Console.WriteLine("1.filter by Status of tasks");
            Console.WriteLine("2.filter by Possible Task for workers");
            Console.WriteLine(".None");

            int choice = int.Parse(Console.ReadLine());
            return choice;
        }


        private static int ShowLevel()
        {
            Console.WriteLine(@"Which level do you want?" +
                              "0 for Beginner" +
                              "1 for Intermediate" +
                              "2 for Expert");


            int level = int.Parse(Console.ReadLine());
            return level;
        }
        private static void readAllW()
        {

            IEnumerable<BO.WorkerInList?> w;
            switch (FilterMenuW())
            {
                case 0: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)0, ShowLevel()); break;
                case 1: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)1); break;
                case 2: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)2); break;
                case 3: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)3); break;
                case 4: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)4); break;
                default: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)4); break;
            }



            foreach (WorkerInList x in w)
            {
                Console.WriteLine(x);
                Console.WriteLine("\n");
            }
        }

        private static int ShowComplexity ()
        {
            Console.WriteLine(@"Which complexity do you want?" +
                              "0 for Tasks of Beginner" +
                              "1 for Tasks of Intermediate" +
                              "2 for Tasks of Expert");


            int complexity = int.Parse(Console.ReadLine());
            return complexity;

        }

        private static int showStatus()
        {
            Console.WriteLine(@"Which status do you want?" +
                              "0 for Unscheduled Tasks" +
                              "1 for Scheduled Tasks" +
                              "2 for OnTrack Tasks"+
                              "3 for Done tasks");
            int MyStatus=int.Parse(Console.ReadLine());
            return MyStatus;
        }

        //private static void printGroups()
        //{
        //   var group= s_bl.Task.GroupByStatus();

        //    foreach(var x in group)
        //    {
        //        Console.WriteLine(x);
        //    }
        //    Console.WriteLine();
        //}


        /// <summary>
        /// this method prints all the tasks data
        /// </summary>
        private static void readAllT()
        {
            IEnumerable<BO.TaskInList> t= s_bl.Task.ReadAllTasks((BO.Filter)4);

                            
                //var tGroup = s_bl.Task.GroupByStatus(); 

                int choice = FilterMenuT();
            switch (choice)
            {
                case 0: t = s_bl.Task.ReadAllTasks((BO.Filter)0, ShowComplexity()); break;
                case 1: t = s_bl.Task.ReadAllTasks((BO.Filter)1,showStatus()); break;
                case 2: t = s_bl.Task.ReadAllTasks((BO.Filter)2); break;
              
                case 3: t = s_bl.Task.ReadAllTasks((BO.Filter)4); break;
                default: t = s_bl.Task.ReadAllTasks((BO.Filter)4); break;
            }
            //add option of print with GroupByStatus....

            //if(choice==4)
            // foreach(var grp in tGroup )
            //    {
            //        Status Currentstatus = grp.Status;
            //        Console.WriteLine($"Status: {grp.Status}");
            //        while ( grp.Status==Currentstatus)
            //        Console.WriteLine(grp);
            //    }

            foreach (TaskInList x in t)
            {
                Console.WriteLine(x);
                Console.WriteLine("\n");
            }
            //IEnumerable<BO.TaskInList> t = s_bl.Task.ReadAllTasks();
            //foreach (BO.TaskInList x in t)
            //{
            //    Console.WriteLine(x);
            //    Console.WriteLine("\n");
            //}

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
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");
                Worker w = s_bl.Worker.ReadWorker(id);

                Console.WriteLine("Enter name,experience, phone number, hourly payment and ID and alias of task of the worker");

                string name = Console.ReadLine();


                BO.WorkerExperience lvl;
                int levelT;
                string level = Console.ReadLine();
                res = int.TryParse(level, out levelT);
                lvl = (BO.WorkerExperience)levelT;

                string phonenumber = Console.ReadLine();


                double cost;
                string SCost = Console.ReadLine();
                res = double.TryParse(SCost, out cost);
                if (res == false)
                    throw new Exception("Cant convert hourly payment of worker from string to double");


                int IdOfTask;
                string sIdTask = (Console.ReadLine());
                res = int.TryParse(sIdTask, out IdOfTask);
                if (res == false)
                    throw new Exception("Cant convert ID Task of worker from string to int");

                string aliasOfTask = Console.ReadLine();

                BO.TaskInWorker TaskOfWorker = new BO.TaskInWorker { Id = IdOfTask, Alias = aliasOfTask };
                BO.Worker WorkerToUp = new BO.Worker { Name = name, Id = id, Level = lvl, PhoneNumber = phonenumber, Cost = cost, Task = TaskOfWorker };

                s_bl.Worker.UpdateWorker(WorkerToUp);

            }
            catch (BO.BlNotActiveException ex)
            {
                Console.WriteLine($"BlNotActiveException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }

            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                Console.WriteLine($"BlInvalidGivenValueException: {ex.Message} ");

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

                BO.Task TaskToUpdate = new BO.Task();
                Console.WriteLine("Enter the ID number of the task you want to update");
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");

                BO.Task t = s_bl.Task.ReadTask(id);//catch
              

                TaskToUpdate.Id = id;

              
                Console.WriteLine("Enter  the description of the task,the alias of the task,the complexity of the task, scheduled start date of the task, deadline of the task, Id of Worker assigned to that Task" +
                    ",erasability of Task, Complete Date of task, remarks on the task and deliverables");
                string  Description=Console.ReadLine();
                string Alias=Console.ReadLine();

                BO.WorkerExperience lvl;
                int levelT;
                string level = Console.ReadLine();
                res= int.TryParse(level,out levelT);
                if (res==false)
                    throw new Exception("Cant convert Level of task from string to enum");
                lvl =(BO.WorkerExperience)levelT;

                DateTime ScheduledDate;
                string schedDate=Console.ReadLine();
                res=DateTime.TryParse(schedDate,out ScheduledDate);
              if(res==false)
                    throw new Exception("Cant convert scheduled date of task from string to DateTime");

                DateTime DeadLineDate;
                string dlDate = Console.ReadLine();
                res = DateTime.TryParse(dlDate, out DeadLineDate);
                if (res == false)
                    throw new Exception("Cant convert dead line date of task from string to DateTime");

                int IdOfWorker;
                string sIdWorker = (Console.ReadLine());
                res = int.TryParse(sIdWorker, out IdOfWorker);
                if (res == false)
                    throw new Exception("Cant convert ID Task of worker from string to int");

                bool eraseable =bool.Parse(Console.ReadLine());

                DateTime CompleteDate;
                string CompDate = Console.ReadLine();
                res = DateTime.TryParse(CompDate, out CompleteDate );
                if (res == false)
                    throw new Exception("Cant convert complete date of task from string to DateTime");


                string remark=Console.ReadLine();   
                string deliverables=Console.ReadLine();


                Console.WriteLine("How many dependencies do you want to add?");

                int NumDep;
                string sNumDep = (Console.ReadLine());
                res = int.TryParse(sNumDep, out NumDep);
                if (res == false)
                    throw new Exception("Cant convert number of dependencies from string to int");


                List<BO.TaskInList?> lst = new List<TaskInList?>();
                for (int i=0;i< NumDep;i++)
                {
                    Console.WriteLine($"enter dependency that task with Id={id} depends on them");
                    int Dep;
                    string sDep = (Console.ReadLine());
                    res = int.TryParse(sDep, out Dep);
                    if (res == false)
                        throw new Exception("Cant convert number of dependencies from string to int");


                    BO.TaskInList? taskDep = new TaskInList { Alias = TaskToUpdate .Alias, Description = TaskToUpdate .Description, Id = TaskToUpdate .Id, Status = TaskToUpdate .Status }; 
                    lst.Add(taskDep);
                    


                }



                TaskToUpdate.Description = Description;
                TaskToUpdate.Alias = Alias;
                TaskToUpdate.Complexity = lvl;
                TaskToUpdate.ScheduledDate = ScheduledDate;
                TaskToUpdate.Deadline = DeadLineDate;
                TaskToUpdate.Worker = new WorkerInTask { Id = IdOfWorker, Name = s_bl.Worker.ReadWorker(IdOfWorker).Name };
                TaskToUpdate.CompleteDate = CompleteDate;
                TaskToUpdate.Remarks = remark;
                TaskToUpdate.Deliverables = deliverables;
                TaskToUpdate.Dependencies = lst;



                s_bl.Task.UpdateTask(TaskToUpdate);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }
            catch(BO.BlInvalidGivenValueException ex)
            {
                Console.WriteLine(ex.Message );
            }
            catch (Exception ex)
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
                int id;
                string sIdWorker = (Console.ReadLine());
                bool res = int.TryParse(sIdWorker, out id);
                if (res == false)
                    throw new Exception("Cant convert ID of worker from string to int");
                s_bl.Worker.RemoveWorker(id);
            }


            catch (BO.BlNotErasableException ex)
            {
                Console.WriteLine($"BlNotErasableException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }

            catch (BO.BlNotActiveException ex)
            {
                Console.WriteLine($"BlNotActiveException: {ex.Message}");

            }

            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        

        private static void AddUpdDateDate()
        {
            try
            {
                Console.WriteLine("Enter Id of the Task you want to add the Start Date and also write the start date");
                int IdOfTask;
                string sIdTask = (Console.ReadLine());
               bool  res = int.TryParse(sIdTask, out IdOfTask);
                if (res == false)
                    throw new Exception("Cant convert ID Task of worker from string to int");


                DateTime StartDate;
                string StrStartDate = Console.ReadLine();
                res = DateTime.TryParse(StrStartDate, out StartDate);
                if (res == false)
                    throw new Exception("Cant convert complete date of task from string to DateTime");


                s_bl.Task.AddOrUpdateStartDate(IdOfTask, StartDate);

            }
            catch(BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException} ");
            }
            catch(BO.BlFalseUpdateDate ex)
            {
                Console.WriteLine($"BO.BlFalseUpdateDate: {ex.Message}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
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
                int id;
                string sIdTask = (Console.ReadLine());
                bool res = int.TryParse(sIdTask, out id);
                if (res == false)
                    throw new Exception("Cant convert ID Task of worker from string to int");

                s_bl.Task.RemoveTask(id);
            }
            catch (BO.BlNotErasableException ex)
            {
                Console.WriteLine($"BlNotErasableException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }

            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException} ");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }


}
