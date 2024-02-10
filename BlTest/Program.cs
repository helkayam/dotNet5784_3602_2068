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
                if (s_bl.Task.GetStatusOfProject() != ProjectStatus.ScheduleDetermination)
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
                else
                {
                    MainPage();

                    int choice = int.Parse(Console.ReadLine());
                    while (choice != 0)
                    {
                        switch (choice)
                        {
                            case 1: updateScheduleDate(); break;
                            default: break;
                        }
                        MainPage();
                        choice = int.Parse(Console.ReadLine());
                    }
                }

            }
            catch (Exception ex)
            {
                /Console.WriteLine(ex.ToString());
            }

        }
        
        static void MainPage()
        {
            if (s_bl.Task.GetStatusOfProject() != ProjectStatus.ScheduleDetermination)
            {
                Console.WriteLine("Select an entity you want to check");
                Console.WriteLine("0.Exit from main menu");
                Console.WriteLine("1.Worker entity");
                Console.WriteLine("2.Task entity");
            }
            else
            {
                Console.WriteLine("select the action you want to do");
                Console.WriteLine("1.Schedule a Task");
            }
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

            if (s_bl.Task.GetStatusOfProject() == ProjectStatus.PlanStage)
            {
                Console.WriteLine("1.Exiting the main menu");
                Console.WriteLine("2.Adding a new task to the list");
                Console.WriteLine("3.Task display by ID");
                Console.WriteLine("4.Display list of all tasks");
                Console.WriteLine("5.Updating existing task data");
                Console.WriteLine("6.Deleting an existing task from the list");
            }
            if (s_bl.Task.GetStatusOfProject() == ProjectStatus.ExecutionStage)
            {
                Console.WriteLine("1.Exiting the main menu");
                Console.WriteLine("2.Task display by ID");
                Console.WriteLine("3.Display list of all tasks");
                Console.WriteLine("4.Updating existing task data");
                Console.WriteLine("5.Report that a task has started ");
                Console.WriteLine("6.Report that a task  is completed");

              
            }


            if (s_bl.Task.GetStatusOfProject() == ProjectStatus.PlanStage)
            {
                int choice = int.Parse(Console.ReadLine());
                if (choice != 1)
                {
                    switch (choice)
                    {
                        case 2: createT(); break;
                        case 3: readT(); break;
                        case 4: readAllT(); break;
                        case 5:updateT(); break;
                        case 6: deleteT(); break;
                        default: break;

                    }
                }
            }
            if (s_bl.Task.GetStatusOfProject() == ProjectStatus.ExecutionStage)
            {
                int choice = int.Parse(Console.ReadLine());
                if (choice != 1)
                {
                    switch (choice)
                    {
                        case 2: readT(); break;
                        case 3: readAllT(); break;
                        case 4:updateT(); break;
                        case 5: AddUpdDateStartDate(); break;
                        case 6: AddCompleteDate(); break;
                        default: break;

                    }
                }
            }



        }
        /// <summary>
        /// this method gets from the user data of the worker we want to add, 
        /// and send a add request to the BL layer
        /// </summary>
        private static void createW()
        {
     
            try
            {
                Console.WriteLine("Enter ID, name,level, phone number ,hourly payment of Worker ");
               

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
                BO.Worker myNewWorker = new BO.Worker { Name = name, Id = id, Level = we, PhoneNumber = phonenumber, Cost = cost };

                if (s_bl.Task.GetStatusOfProject() == ProjectStatus.ExecutionStage)
                {
                    Console.WriteLine("Enter Id of Task for the worker");
                    int IdOfTask;
                    string sIdTask = (Console.ReadLine());
                    res = int.TryParse(sIdTask, out IdOfTask);
                    if (res == false)
                        throw new Exception("Cant convert ID Task of worker from string to int");
                    string aliasOfTask = s_bl.Task.ReadTask(IdOfTask).Alias;


                    BO.TaskInWorker TaskOfWorker = new BO.TaskInWorker { Id = IdOfTask, Alias = aliasOfTask };
                    myNewWorker.Task = TaskOfWorker;
                }


                s_bl.Worker.AddWorker(myNewWorker);
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine($"BlAlreadyExistsException: {ex.Message} \n Inner Exception: {ex.InnerException.Message} ");

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
        /// this method gets from the user data of the task we want to add, 
        /// and send a add request to the BL layer
        /// </summary>
        private static void createT()
        {
            try
            {
              
                if(s_bl.Task.GetStatusOfProject()==ProjectStatus.PlanStage)
                  
                Console.WriteLine("Enter description, alias, erasable of the task, Remarks  Complexity of the task and Required effort Time  ");
                string description = Console.ReadLine();
                string alias = Console.ReadLine();

                DateTime creatingDate=DateTime.Now;
              

                bool erasable = bool.Parse(Console.ReadLine());


                string Remarks = Console.ReadLine();
               

                int complexity;
                string sComplexity = Console.ReadLine();
                bool res = int.TryParse(sComplexity, out complexity);
                if (res == false)
                    throw new Exception("Cant convert Complexity from string to int");
                BO.WorkerExperience TaskComplexity = (BO.WorkerExperience)complexity;


                TimeSpan RET = TimeSpan.Parse(Console.ReadLine());


                Console.WriteLine("Enter number of dependent tasks");
                int numOfDep;
                string SNumOfDep = Console.ReadLine();
                res = int.TryParse(SNumOfDep, out numOfDep);
                if (res == false)
                    throw new Exception("Cant convert number of dependent tasks from string to int");

                List<TaskInList> depTasks = new List<TaskInList>();
                for (int i = 0; i < numOfDep; i++)
                {
                    Console.WriteLine("Enter ID of dependent tasks");
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
                    ScheduledDate = null,
                    Deadline = null,
                    Remarks = Remarks,
                    Worker = null,
                    Complexity = TaskComplexity,
                    RequiredEffortTime=RET
                    
                };

                

                s_bl.Task.AddTask(newTask);
            }


              catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine($"BlAlreadyExistsException: {ex.Message} \n Inner Exception: {ex.InnerException.Message} ");

            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                Console.WriteLine($"BlInvalidGivenValueException: {ex.Message} ");

            }
            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message}");
            }
            catch (BO.BlForbiddenActionException ex)
            {
                Console.WriteLine($"BlForbiddenActionException: {ex.Message} ");

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
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message}");
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
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message}");
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

            IEnumerable<BO.Worker?> w;
            switch (FilterMenuW())
            {
                case 0: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)0, ShowLevel()); break;
                case 1: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)1); break;
                case 2: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)2); break;
                case 3: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)3); break;
                case 4: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)4); break;
                default: w = s_bl.Worker.ReadAllWorkers((BO.FilterWorker)4); break;
            }



            foreach (Worker x in w)
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

        
        private static int ComplexityTaskForWorker()
        {
            Console.WriteLine("Enter Id of worker you want to check wich Task he can chose");
            int IdW=int.Parse(Console.ReadLine());
           WorkerExperience  Complexity = s_bl.Worker.ReadWorker(IdW).Level;
            return (int)Complexity;
        }

        /// <summary>
        /// this method prints all the tasks data
        /// gets from the user wich filter he wants to perform
        /// and print the task according to the filter
        /// </summary>
        private static void readAllT()
        {
            IEnumerable<BO.TaskInList> t = s_bl.Task.ReadAllTasks((BO.Filter)4);


            //var tGroup = s_bl.Task.GroupByStatus(); 

            int choice = FilterMenuT();
            switch (choice)
            {
                case 0: t = s_bl.Task.ReadAllTasks((BO.Filter)0, ShowComplexity()); break;
                case 1: t = s_bl.Task.ReadAllTasks((BO.Filter)1, showStatus()); break;
                case 2: t = s_bl.Task.ReadAllTasks((BO.Filter)2, ComplexityTaskForWorker()); break;

                case 3: t = s_bl.Task.ReadAllTasks((BO.Filter)4); break;
                default: t = s_bl.Task.ReadAllTasks((BO.Filter)4); break;
            }


            foreach (TaskInList x in t)
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
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");
                Worker w = s_bl.Worker.ReadWorker(id);

                Console.WriteLine("Enter name,experience, phone number, hourly payment ");

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

                BO.Worker MyUpdWorker = new BO.Worker { Name = name, Id = id, Level = lvl, PhoneNumber = phonenumber, Cost = cost };

                if (s_bl.Task.GetStatusOfProject() == ProjectStatus.ExecutionStage)
                {
                    Console.WriteLine("Do you want to alocate a task to the worker? Y/N");
                        char c=char.Parse(Console.ReadLine());
                    if (c == 'Y')
                    {
                        Console.WriteLine("Enter Id of task you want to allocate to the worker ");
                        int IdOfTask;
                        string sIdTask = (Console.ReadLine());
                        res = int.TryParse(sIdTask, out IdOfTask);
                        if (res == false)
                            throw new Exception("Cant convert ID Task of worker from string to int");
                        TaskInWorker t = new TaskInWorker() { Alias = s_bl.Task.ReadTask(IdOfTask).Alias, Id = IdOfTask };
                        MyUpdWorker.Task = t;
                    }

                }


                s_bl.Worker.UpdateWorker(MyUpdWorker);

            }
            catch (BO.BlNotActiveException ex)
            {
                Console.WriteLine($"BlNotActiveException: {ex.Message} \n Inner Exception: {ex.InnerException.Message} ");

            }

            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message} ");

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
        /// this method ask from the user the id of the task he want to schedule 
        /// and also the schedule date and send a request to bl layer.
        /// </summary>
        /// <exception cref="Exception">throw exception if he cant convert chedule date from string to datetime</exception>
        private static void updateScheduleDate()
        {
            try
            {
                Console.WriteLine("Enter Id of task you want to schedule and enter schedule date");
                int id;
                string Sid = (Console.ReadLine());
                bool res = int.TryParse(Sid, out id);
                if (res == false)
                    throw new Exception("Cant convert Id of worker from string to int");

                DateTime sdt;
                string date = (Console.ReadLine());
                 res = DateTime.TryParse(date,out  sdt);
                if (res == false)
                    throw new Exception("Cant convert Schedule date of worker from string to DateTime");

                s_bl.Task.UpdateScheduleDate(id, sdt);


            }
            catch(BO.BlFalseUpdateDate e)
            {
                Console.WriteLine($" BO.BlFalseUpdateDate: {e.Message}" );
            }
            catch (BO.BlInvalidGivenValueException e)
            {
                Console.WriteLine($" BO.BlInvalidGivenValueException: {e.Message}");
            }
            catch (BO.BlDoesNotExistException e)
            {
                Console.WriteLine($" BO.BlInvalidGivenValueException: {e.Message} \n Inner Exception: {e.InnerException.Message}");
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

              

                TaskToUpdate.Id = id;

              
                Console.WriteLine("Enter  the description of the task, alias of the task,the complexity of the task, " +
                    " remarks on the task and erasability of Task");
                string  Description=Console.ReadLine();
                string Alias=Console.ReadLine();

                BO.WorkerExperience lvl;
                int levelT;
                string level = Console.ReadLine();
                res= int.TryParse(level,out levelT);
                if (res==false)
                    throw new Exception("Cant convert Level of task from string to enum");
                lvl =(BO.WorkerExperience)levelT;

                string remark = Console.ReadLine();
                bool eraseable = bool.Parse(Console.ReadLine());


                if (s_bl.Task.GetStatusOfProject() == ProjectStatus.ExecutionStage)
                {

                    Console.WriteLine("Enter Deadline of task, Id of worker assigned to the task and deliverables");
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



                    string deliverables = Console.ReadLine();


                    TaskToUpdate.Deadline = DeadLineDate;
                    TaskToUpdate.Worker = new WorkerInTask { Id = IdOfWorker, Name = s_bl.Worker.ReadWorker(IdOfWorker).Name };
                    TaskToUpdate.Deliverables = deliverables;

                }
                else
                {

                    Console.WriteLine("enter required effort time");
                        TimeSpan re = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine("How many dependencies do you want to add?");

                    int NumDep;
                    string sNumDep = (Console.ReadLine());
                    res = int.TryParse(sNumDep, out NumDep);
                    if (res == false)
                        throw new Exception("Cant convert number of dependencies from string to int");


                    List<BO.TaskInList?> lst = new List<TaskInList?>();
                    for (int i = 0; i < NumDep; i++)
                    {
                        Console.WriteLine($"enter dependency that task with Id={id} depends on them");
                        int Dep;
                        string sDep = (Console.ReadLine());
                        res = int.TryParse(sDep, out Dep);
                        if (res == false)
                            throw new Exception("Cant convert number of dependencies from string to int");


                        BO.TaskInList? taskDep = new TaskInList { Alias = TaskToUpdate.Alias, Description = TaskToUpdate.Description, Id = TaskToUpdate.Id, Status = TaskToUpdate.Status };
                        lst.Add(taskDep);



                    }
                    TaskToUpdate.Dependencies = lst;

                }



                TaskToUpdate.Description = Description;
                TaskToUpdate.Alias = Alias;
                TaskToUpdate.Complexity = lvl;
                TaskToUpdate.Eraseable = eraseable;
                TaskToUpdate.Remarks = remark;



                s_bl.Task.UpdateTask(TaskToUpdate);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message} ");

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
                Console.WriteLine($"BlNotErasableException: {ex.Message} \n Inner Exception: {ex.InnerException.Message } ");

            }

            catch (BO.BlNotActiveException ex)
            {
                Console.WriteLine($"BlNotActiveException: {ex.Message}");

            }

            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message } ");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        ///this method ask from the use the id of the task 
        ///he want to report that is completed, and send a request to the bl layer 
        /// </summary>
        private static void AddCompleteDate()
        {
            try
            {
                Console.WriteLine("Enter Id of the Task you want to report that is completed");
                int IdOfTask;
                string sIdTask = (Console.ReadLine());
                bool res = int.TryParse(sIdTask, out IdOfTask);
                if (res == false)
                    throw new Exception("Cant convert ID Task from string to int");
            }
            catch(BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message} ");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);   
            }



        }

        //this method ask from the use the id of the task he want to report that has started, and send a request to the bl layer 
        /// <summary>
        /// this method ask from the use the id of the task he want to report that has started, 
        /// and send a request to the bl layer 
        /// </summary>
        private static void AddUpdDateStartDate()
        {
            try
            {
                Console.WriteLine("Enter Id of the Task you want to report that has started");
                int IdOfTask;
                string sIdTask = (Console.ReadLine());
               bool  res = int.TryParse(sIdTask, out IdOfTask);
                if (res == false)
                    throw new Exception("Cant convert ID Task of worker from string to int");

                s_bl.Task.AddOrUpdateStartDate(IdOfTask);

            }
            catch(BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} \n Inner Exception: {ex.InnerException.Message } ");
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
                Console.WriteLine($"BlNotErasableException: {ex.Message} \n Inner Exception: {ex.InnerException.Message } ");

            }

            catch (BO.BlDoesNotExistException ex)
            {
                Console.WriteLine($"BlDoesNotExistException: {ex.Message} ");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }


}
