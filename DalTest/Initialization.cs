namespace DalTest;
using DalApi;
using DO;
using System;


public static class Initialization
{
    private static IDependency? s_dalDependency;
    private static IWorker? s_dalWorker;
    private static ITask? s_dalTask;

    private static readonly Random s_rand = new();

    private static void createWorker()
    {
        
        string[] phonePrefix = { "050", "051", "052", "053", "054", "056", "058" };

        string[] workerNames = { "Hanna Cohen", "Ziv Peretz", "Tali Levi", "Yair Israel", "Dalya Amar", "Ben Klein" };

        foreach(var _name in workerNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000 );
            while (s_dalWorker!.Read(_id) != null);

            int _lvl = s_rand.Next(0, 2);
            

            string _phonePrefix = phonePrefix[s_rand.Next(0, 6)];
            string _number = (s_rand.Next(0, 9999999).ToString());
            string _phoneNumber = _phonePrefix + _number;

            
            double _cost = s_rand.NextDouble()+s_rand.Next(31, 200);

            Worker newWrk = new(_id, (WorkerExperience)_lvl, _name, _phoneNumber, _cost);
            s_dalWorker.Create(newWrk);


        }
    }
}
