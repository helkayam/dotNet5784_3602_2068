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

    private static void createWorker()
    {

        string[] phonePrefix = { "050", "051", "052", "053", "054", "056", "058" };

        string[] workerNames = { "Hanna Cohen", "Ziv Peretz", "Tali Levi", "Yair Israel", "Dalya Amar", "Ben Klein" };

        foreach (var _name in workerNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Worker.Read(_id) != null);

            int _lvl = s_rand.Next(0, 2);


            string _phonePrefix = phonePrefix[s_rand.Next(0, 6)];
            string _number = (s_rand.Next(0, 9999999).ToString());
            string _phoneNumber = _phonePrefix + _number;


            double _cost = s_rand.NextDouble() + s_rand.Next(31, 200);

            Worker newWrk = new(_id, (WorkerExperience)_lvl, _name, _phoneNumber, _cost);
            s_dal!.Worker.Create(newWrk);


        }
    }

    private static void createTasks()
    {
        string[] tasks = { "fireworks","souvenirs" , "Children's corner", "Activities for adults", "Alcohol stand",
            "Snack stand","food stands","gifts for children" ,"performances","flags","Sports and fitness station",
        "Corner of knowledge and history","photo station","Knowledge and training station","Digital advertising",
        "Government agencies station", "Public transport route","Waste disposal area","Rental of equipment and accessories",
        "Supervision of safety level","Art workshops","seating area","Photogenic station","family activities",
        "Jewelry manufacturing station","ground activities","Determining the place of the event","Events Manager",
        "music","Open areas and sports","Event documentation","First aid"
        };
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
            _description = tasks[i];
            _complexity = s_rand.Next(0, 2);
            DateTime start = new DateTime(2023, 12, 1);
            DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            int range = (end - start).Days;
            _createdAtDate = start.AddDays(s_rand.Next(range)).AddHours(s_rand.Next(0, 24)).AddMinutes(s_rand.Next(0, 60)).AddSeconds(s_rand.Next(0, 60));
            Task newTask = new Task(_alias, (WorkerExperience)(_complexity), _description, null, null);
            newTask.CreatedAtDate = _createdAtDate;
            s_dal!.task.Create(newTask);
        }
    }

    private static void createDependencies()
    {
        int[,] halpMatrix = { { 6, 3 }, { 13, 3 }, { 31, 15 }, { 4, 28 }, { 5, 28 }, { 6, 28 }, { 7, 28 }, { 9, 28 }, { 21, 24 }, { 28, 10 }, { 9, 1 }, { 2, 13 },
            { 8, 3 }, { 11, 4 }, { 31, 2 }, { 22, 9 }, { 20, 9 }, { 12, 4 }, { 22, 3 }, { 28, 27 }, { 28, 29 }, { 28, 19 }, { 19, 10 }, { 28, 6 }, { 20, 28 }
            , { 19, 9 }, { 28, 7 }, { 14, 12 }, { 21, 3 }, { 30, 4 }, { 27, 18 }, { 28, 24 }, { 28, 5 }, { 28, 20 }, { 7, 6 }, { 28, 26 }, { 27, 26 }, { 13, 15 }
            , { 28, 3 }, { 27, 17 }, { 28, 32 }, { 27, 30 }, { 25, 3 }, { 29, 9 }, { 14, 16 }, { 3, 29 } };
        for (int i = 0; i < 45; i++)
        {
            Dependency dNew = new Dependency(halpMatrix[i, 0], halpMatrix[i, 1]);
            s_dal!.Dependency.Create(dNew);
        }
    }


    public static void Do(IDal dal) 
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!");
        createDependencies();
        createTasks();
        createWorker();
    }


}









