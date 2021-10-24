using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace Assignment6._0
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegisterUserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RegisterUserService.svc or RegisterUserService.svc.cs at the Solution Explorer and start debugging.
    public class RegisterUserService : IRegisterUserService
    {
        public class UsersRootObject
        {
            public User[] users { get; set; } // Array of users
        }

        // User class object
        public class User
        {
            public string StudentName { get; set; }
            public string Password { get; set; }
            public string StudentID { get; set; }
            public List<string> StudentCourses { get; set; }
        }

        // Accepts StudentName and password as parameters. Encrypts passwords and stores in a JSON file.
        // Creates an account; checks if the StudentName already exists
        public string createAccount(string StudentName, string password)
        {
            User newUser = new User(); // User object for new user
            UsersRootObject usersObj = new UsersRootObject(); // Object of user
            List<User> usersList = new List<User>(); // List of users to read in existing data and add new users
            string json; // for the final JSON formatted list of users
            Boolean exists = false; // boolean value to check if the StudentName exists
            int id = -1; // student ID
            byte[] pwd; // byte array to store the encrypted password into
            string encryptedPass = ""; // string to store the encrypted password into


            string path = HttpRuntime.AppDomainAppPath + "\\users_list.json"; // File path to user credentials 

            try
            {
                string jsonData = File.ReadAllText(path); // reads in the JSON file into a string

                usersObj = JsonConvert.DeserializeObject<UsersRootObject>(jsonData); // transfers jsonData to the usersObj

                if (usersObj.users != null) // makes sure that there is at least one existing user to iterate through accounts
                {
                    usersList = usersObj.users.ToList<User>(); // transfers users to a List<User>

                    foreach (User user in usersList) // iterates through the users
                    {
                        if (user.StudentName == StudentName) // checks if the StudentName already exists
                        {
                            exists = true;
                        }
                    }
                }

                if (!exists) // If StudentName doesn't already exist
                {
                    pwd = Encoding.ASCII.GetBytes(password); // Encrypts the password

                    // Loop converts byte array to a string
                    foreach (byte digit in pwd)
                    {
                        encryptedPass += digit;
                    }

                    newUser.StudentName = StudentName;
                    newUser.Password = encryptedPass;
                    newUser.StudentCourses = new List<string>();
                    // generate Student ID
                    Boolean present = false;
                    Random rnd = new Random();
                    while (true)
                    {
                        id = rnd.Next(10000, 100000);
                        foreach (User user in usersList) // iterates through the users
                        {
                            if (user.StudentID == id.ToString()) // checks if the StudentID already exists
                            {
                                present = true;
                            }
                        }
                        if (!present)
                        {
                            newUser.StudentID = id.ToString();
                            break;
                        }
                    }
                     
                    //newUser.StudentCourses = new string[];
                    usersList.Add(newUser); // adds the new user to the user list

                    usersObj.users = usersList.ToArray<User>(); // Converts the list to a User object array
                    json = JsonConvert.SerializeObject(usersObj, Formatting.Indented); // Converts object to JSON string
                    File.WriteAllText(path, json); // Writes JSON data to the file
                }
            }
            finally
            {

            }
            return id.ToString();
        }
    }
}
