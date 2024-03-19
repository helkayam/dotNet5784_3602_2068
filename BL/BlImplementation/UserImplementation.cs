
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace BlImplementation;

internal class UserImplementation : BlApi.IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddUser(BO.User user)
    {
        DO.User newUser = new DO.User() { Id = user.Id, Name = user.Name, Email = user.Email, PassWord = user.Password, UserName = user.UserName };
        if ((int)_dal.Worker.Read(user.Id).Level == 3)
            newUser.IsAdmin = true;
        else newUser.IsAdmin = false;

        try
        {
            if (user.Name == null)
                throw new BO.BlInvalidGivenValueException("The name field is a required field");
            if (user.Id == null)
                throw new BO.BlInvalidGivenValueException("The Id field is a required field");
            if (user.Email == null)
                throw new BO.BlInvalidGivenValueException("The email field is a required field");
            if (user.UserName == null)
                throw new BO.BlInvalidGivenValueException("The user name field is a required field");
            if (user.Password == null)
                throw new BO.BlInvalidGivenValueException("The name password is a required field");
            if (newUser.PassWord.Length >= 8 && newUser.PassWord.Length <= 10 && newUser.PassWord.Any(t => (int)t >= 65 && (int)t <= 90) && newUser.PassWord.Any(t => (int)t >= 97 && (int)t <= 122) && newUser.PassWord.Any(t => (int)t >= 48 && (int)t <= 57))
            {
                if (newUser.Email.Any(t => t == 64) && newUser.Email.Contains("gmail.com") && (newUser.Email.Any(t => (int)t >= 65 && (int)t <= 90) || newUser.Email.Any(t => (int)t >= 97 && (int)t <= 122)))
                {
                    if (newUser.UserName.Length >= 6 && newUser.UserName.Length <= 20 && (newUser.UserName.Any(t => (int)t >= 65 && (int)t <= 90) || newUser.UserName.Any(t => (int)t >= 97 && (int)t <= 122)) && newUser.UserName.Any(t => (int)t >= 48 && (int)t <= 57))
                        _dal.User.Create(newUser);
                    else
                        throw new BO.BlInvalidGivenValueException($"This user name={user.UserName} is not valid");

                }
                else
                    throw new BO.BlInvalidGivenValueException($"This email={user.Email} is not valid");
            }
            else
                throw new BO.BlInvalidGivenValueException($"This password={user.Password} is not valid");

        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with UserName={user.UserName} already exists", ex);
        }


    }

    public void RemoveUser(string userName)
    {
        try
        {
            _dal.User.Delete(userName);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with UserName={userName} doesnt exists", ex);
        }
    }

    public IEnumerable<BO.User> ReadAllUsers()
    {
        return from users in _dal.User.ReadAll()
               select new BO.User() { Id = Convert.ToInt32(users.Id), Name = users.Name, Email = users.Email, IsAdmin = users.IsAdmin, UserName = users.UserName, Password = users.PassWord };
    }

    public BO.User? ReadUser(string userName, bool throwexception = false)
    {
        try
        {
            if (throwexception == true)
                _dal.User.Read(userName, true);
            else
                _dal.User.Read(userName, false);


        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with UserName={userName} doesnt exists", ex);
        }

        DO.User user = _dal.User.Read(userName)!;
        if (user == null)
            return null;
        return new BO.User { Id = Convert.ToInt32(user.Id), Name = user.Name, Email = user.Email, IsAdmin = user.IsAdmin, Password = user.PassWord, UserName = user.UserName, };

    }

    public void UpdateUser(BO.User user)
    {

         DO.User updUser = new DO.User() { Id = user.Id, Name = user.Name, Email = user.Email, PassWord = user.Password, UserName = user.UserName};
        if ((int)_dal.Worker.Read(user.Id).Level == 3)
            updUser.IsAdmin = true;
        else
            updUser.IsAdmin = false;
        if (user.Name == null)
            updUser = updUser with { Name = _dal.User.Read(user.UserName).Name };
            if (user.Id == null)
            updUser = updUser with { Id = _dal.User.Read(user.UserName).Id };
        if (user.Email == null)
            updUser = updUser with { Email = _dal.User.Read(user.UserName).Email };
        if (user.UserName == null)
            updUser = updUser with { UserName = _dal.User.Read(user.UserName).UserName };
        if (user.Password == null)
            updUser = updUser with { PassWord = _dal.User.Read(user.UserName).PassWord };
        try
        {

            _dal.User.Read(user.UserName);
            if (updUser.PassWord.Length >= 8 && updUser.PassWord.Length <= 10 && updUser.PassWord.Any(t => (int)t >= 65 && (int)t <= 90) && updUser.PassWord.Any(t => (int)t >= 97 && (int)t <= 122) && updUser.PassWord.Any(t => (int)t >= 48 && (int)t <= 57))
            {
                if (updUser.Email.Any(t => t == 64) && updUser.Email.Contains("gmail.com") && (updUser.Email.Any(t => (int)t >= 65 && (int)t <= 90) || updUser.Email.Any(t => (int)t >= 97 && (int)t <= 122)))
                {
                    if (updUser.UserName.Length >= 6 && updUser.UserName.Length <= 20 && (updUser.UserName.Any(t => (int)t >= 65 && (int)t <= 90) || updUser.UserName.Any(t => (int)t >= 97 && (int)t <= 122)) && updUser.UserName.Any(t => (int)t >= 48 && (int)t <= 57))
                        _dal.User.Create(updUser);
                    else
                        throw new BO.BlInvalidGivenValueException($"This user name={user.UserName} is not valid");

                }
                else
                    throw new BO.BlInvalidGivenValueException($"This email={user.Email} is not valid");
            }
            else
                throw new BO.BlInvalidGivenValueException($"This password={user.Password} is not valid");


        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"User with userName={user.UserName} does Not exist", ex);

        }


    }

    public bool checkExistId(int id)
    {
        var users = ReadAllUsers();
        foreach(var user in users) 
        {
            if(user.Id == id) 
                return true;
        }
        return false;
    }

    public int SendEmail(string email)
    {
        int randCode = 0;
        Random rand = new Random();
        randCode = rand.Next(1000, 10000);


        MailMessage mail = new MailMessage();
        mail.To.Add(email);
        mail.From = new MailAddress("d9349019@gmail.com");
        mail.Subject = @"Verification Code";
        mail.Body= @"Your verification code is: "+randCode.ToString() + "\n *The code is valid for 30 seconds";
        mail.IsBodyHtml = true;
        SmtpClient smpt = new SmtpClient();
        smpt.Host = "smtp.gmail.com";
        smpt.Credentials=new NetworkCredential("d9349019@gmail.com", "dotNet20683602");
        smpt.DeliveryMethod = SmtpDeliveryMethod.Network;
        smpt.EnableSsl = true;
        smpt.Port = 587;
        try
        {
            smpt.Send(mail);
            return randCode;
        }
        catch (SmtpFailedRecipientException ex)
        {
            throw new("There was a problem sending the email to this user");
        }
        catch (SmtpException ex) 
        {
            throw new("Unable to connect to GMAIL server");
        }
        catch(Exception e)
        {
            throw new (e.Message);
        }

    }



}
