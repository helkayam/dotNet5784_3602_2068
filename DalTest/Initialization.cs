namespace DalTest;
using DalApi;
using DO;
using System;
using System.Reflection.Emit;
using System.Xml.Linq;


public static class Initialization
{
    private static IDal? s_dal; 
    

    private static readonly Random s_rand = new();

    /// <summary>
    /// this method create random workers,and initalize them 
    /// </summary>
    private static void createWorker()
    {
        //string array that have prefix of the phone inside 
        string[] phonePrefix = { "050", "051", "052", "053", "054", "056", "058" };

        //string array of names
        string[] workerNames = { "Hanna Cohen", "Ziv Peretz", "Tali Levi", "Yair Israel", "Dalya Amar", "Ben Klein" };

        //for every name in the string array of names
        foreach (var _name in workerNames)
        {
            int _id;
            //we get a random id, and if its found in the list, we again get another random, until the id we get is not in the list
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Worker.Read(_id) != null);

            //level (experience) of the worker. we get a random number between 0 and 2
            int _lvl = s_rand.Next(0, 2);


            //get a random prefix for the phone number
            string _phonePrefix = phonePrefix[s_rand.Next(0, 6)];

            //get a random number for the phone number
            string _number = (s_rand.Next(0, 9999999).ToString());

            //the phone number will be the prefix+the number
            string _phoneNumber = _phonePrefix + _number;

            //cost for hour of the worker (salary for one hour). we get a random double number between 31 and 200
            double _cost = s_rand.NextDouble() + s_rand.Next(31, 200);
           
            //create a object of type worker with the data we get before 
            Worker newWrk = new(_id, (WorkerExperience)_lvl, _name, _phoneNumber, _cost);

            //if the worker is not an expert, he is erasable
            if(_lvl !=2)
                newWrk.Eraseable= true; 

            //create the worker and add to the list
             s_dal!.Worker.Create(newWrk);


        }
    }

    /// <summary>
    /// this method create Tasks,and initalize them
    /// </summary>
    private static void createTasks()
    {
        //alias of 31 tasks.
        string[] tasks = { "fireworks","souvenirs" , "Children's corner", "Activities for adults", "Alcohol stand",
            "Snack stand","food stands","gifts for children" ,"performances","flags","Sports and fitness station",
        "Corner of knowledge and history","photo station","Knowledge and training station","Digital advertising",
        "Government agencies station", "Public transport route","Waste disposal area","Rental of equipment and accessories",
        "Supervision of safety level","Art workshops","seating area","Photogenic station","family activities",
        "Jewelry manufacturing station","ground activities","Determining the place of the event","Events Manager",
        "music","Open areas and sports","Event documentation","First aid"
        };

        //description of the 31 tasks
        string[] Descriptions = { "Take care of 4 types of fireworks, for 9 o'clock in the evening",
        "Take care of 5 types of souvenirs from the event",
        "Take care of 10 positions of experiential activities for children aged 5-12",
        "Take care of 2 positions of adult activities",
        "Take care of the position with beer, vodka, whiskey, and energy drinks. In addition, you will take care of the employees in the position and age enforcement"
         ,"Take care of popcoren position, Potato Chips. Beasley in 5 different positions",
        "Take care of 5 different stalls of sausage in a bun, chips, falafel, shawarma and schnitzel",
        "Organize 3 interesting and original gifts for children in honor of Independence Day",
        "Take care of 2 singers for the evening, including a guest singer, a popular singer and a singer for the older ages",
        "Make sure to order Independence Day flags in different sizes",
        "Organize sports activities, tournaments and fitness stations",
        "Creating a corner that provides information and explanations about the history of Independence Day",
        "An area for photographs and a combination of treats such as headphones and photo treats",
        "and tutorials on issues related to independence",
        "Take care of developing digital content and maintaining activity on social networks",
        "Make sure there is an area of advice and solutions in government matters",
        "Free shuttles or public transportation stations should be allowed to receive the participants",
        "Take care of establishing an area for waste disposal and recycling",
        "Taking care of the props, equipment and infrastructure needed for the event",
        "The security of the security forces in the area must be taken care of",
        "Production of art workshops, such as painting, creating costumes and more",
        "Creating bright and pleasant sitting areas",
        "Setting up a photo station with interesting backgrounds",
        "Activities suitable for families with children",
        "Workshops to create unique jewelry",
        "Workshops and unique land activities",
        "Choosing a place suitable for the number of participants and the type of activities",
        "Viewing and operating an event management team to supervise the production",
        "You have to take care of unique and diverse and happy music and order a suitable DJ",
        "Creating open grass for sports activities and games",
        "Photographers should be taken care of to document the entire event in a broad way",
        "Ensure that there are MDA teams in the area"
        };

        string _alias;
        int _complexity;
        string _description;
        DateTime _createdAtDate;


        for (int i = 0; i < 31; i++)
        {

            _alias = tasks[i];
            _description = Descriptions[i];

            //complexity we get a random number between 0 and 2
            _complexity = s_rand.Next(0, 2);

            //start date of the time we can create a task.
            DateTime start = new DateTime(2024, 1, 1);

            //the end of the time we can create a task
            DateTime end = DateTime.Today.AddDays(-1);

            //random number of days required to do the task
            TimeSpan Ret = TimeSpan.FromDays(s_rand.Next(0, 7));


              
            //range=numbers of the days beween start and end
            int range = ((end - start).Days);

            //then we get a random date between start and end 
            _createdAtDate = start.AddDays(s_rand.Next(range)).AddHours(s_rand.Next(0, 24)).AddMinutes(s_rand.Next(0, 60)).AddSeconds(s_rand.Next(0, 60));
           
          
            //create a new object of type task
            Task newTask = new Task(_alias, (WorkerExperience)(_complexity), _description, 0,null, null);
            
            newTask.CreatedAtDate = _createdAtDate;
            newTask.RequiredEffortTime = Ret;

            //Create the task and add to the list
            s_dal!.Task.Create(newTask);
        }
    }


    /// <summary>
    /// this method create depdnencies and initalize them
    /// </summary>
    private static void createDependencies()
    {
        //matrix where we write the id of dependent id and dependon task
        int[,] halpMatrix = { { 6, 3 }, { 13, 3 }, { 31, 15 }, { 4, 28 }, { 5, 28 }, { 6, 28 }, { 7, 28 }, { 9, 28 }, { 21, 24 }, { 28, 10 }, { 9, 1 }, { 2, 13 },
            { 8, 3 }, { 11, 4 }, { 31, 2 }, { 22, 9 }, { 20, 9 }, { 12, 4 }, { 22, 3 }, { 28, 27 }, { 28, 29 }, { 28, 19 }, { 19, 10 }, { 28, 6 }, { 20, 28 }
            , { 19, 9 }, { 28, 7 }, { 14, 12 }, { 21, 3 }, { 30, 4 }, { 27, 18 }, { 28, 24 }, { 28, 5 }, { 28, 20 }, { 7, 6 }, { 28, 26 }, { 27, 26 }, { 13, 15 }
            , { 28, 3 }, { 27, 17 }, { 28, 32 }, { 27, 30 }, { 25, 3 }, { 29, 9 }, { 14, 16 }, { 3, 29 } };
       
        //now we go over the matrix and add the dependencies to the list of dependencies
        for (int i = 0; i < 45; i++)
        {
            Dependency dNew = new Dependency(halpMatrix[i, 0]-1, halpMatrix[i, 1]-1);
            s_dal!.Dependency.Create(dNew);
        }
    }


    public static void Do() 
    {

        s_dal = DalApi.Factory.Get;//stage 4
        
        //initialize dependencies
        createDependencies();

        //initialize the tasks
        createTasks();

        //initialize the workers
        createWorker();
    }


}









