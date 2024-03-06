using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class ScheduleImplementation:BlApi.ISchedule
    {

        private DalApi.IDal _dal = DalApi.Factory.Get;

        private readonly Bl _bl;
        internal ScheduleImplementation(Bl bl) => _bl = bl;

        /// <summary>
        /// This is a method that receives a date as a parameter and sends it to a class in the DAL layer that takes care of initializing the start date of the project 
        /// on the received date
        /// </summary>
        /// <param name="startDateProject"> Date to start the StartDateOfProject field </param>
        public void UpdateStartProjectDate(DateTime startDateProject)
        {
            if (startDateProject >= _bl.Clock)
                _dal.Schedule.UpdateStartDateProject(startDateProject);
            else
                throw new BO.BlInvalidGivenValueException($"error:start date of project is before current date {_dal.Schedule.GetCurrentDate()}");
        }

        public void UpdateCurrentDate(DateTime currentdt)
        {
            _dal.Schedule.UpdateCurrentDate(currentdt);
        }

        public void UpdateEndDateProjectDate(DateTime endDateProject)
        {
            if (endDateProject <= _bl.Clock)
                _dal.Schedule.UpdateEndDateProject(endDateProject);
            else
                throw new BO.BlInvalidGivenValueException($"error:End date of project is before current date {_dal.Schedule.GetCurrentDate()}");
        }

        public DateTime? getStartDateProject()
        {
            try
            {
                return _dal.Schedule.GetStartDateProject();
            }
            catch(DO.DalDoesNotExistException ex) 
            {
                throw new BO.BlDoesNotExistException("The Start date of the project has not yet been updated",ex);
            
            }


        }
        public DateTime? getEndDateProject()
        {
            try
            {
                return (DateTime)_dal.Schedule.getEndDateProject();
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException("The End date of the project has not yet been updated", ex);

            }


        }

        public DateTime? GetCurrentDate()
        {
            
                return _bl.Clock;

        }

      
    }

}
