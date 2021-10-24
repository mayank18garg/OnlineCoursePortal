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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DeleteUserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DeleteUserService.svc or DeleteUserService.svc.cs at the Solution Explorer and start debugging.
    public class DeleteUserService : IDeleteUserService
    {
        public string deleteStudent(string studentID)
        {

            User newUser = new User(); // User object for new user
            UsersRootObject usersObj = new UsersRootObject(); // Object of user
            List<User> usersList = new List<User>(); // List of users to read in existing data and add new users
            string json; // for the final JSON formatted list of users
            Boolean exists = false; // boolean value to check if the StudentName exists
            


            string path = HttpRuntime.AppDomainAppPath + "\\users_list.json"; // File path to user credentials 
            string coursespath = HttpRuntime.AppDomainAppPath + "\\courses_list.json"; // File path to user credentials 

            try
            {
                string jsonData = File.ReadAllText(path); // reads in the JSON file into a string

                usersObj = JsonConvert.DeserializeObject<UsersRootObject>(jsonData); // transfers jsonData to the usersObj

                if (usersObj.users != null) // makes sure that there is at least one existing user to iterate through accounts
                {
                    usersList = usersObj.users.ToList<User>(); // transfers users to a List<User>

                    foreach (User user in usersList) // iterates through the users
                    {
                        if (user.StudentID == studentID) // checks if the StudentName already exists
                        {
                            exists = true;
                        }
                    }
                }
                else
                {
                    return "No students found.";
                }

                if (exists) 
                {

                    var itemToRemove = usersList.SingleOrDefault(r => r.StudentID == studentID);
                    if (itemToRemove != null)
                    {
                        bool isEmpty = !itemToRemove.StudentCourses.Any();
                        if (!isEmpty)
                        {
                            List<Course> coursesList = new List<Course>();
                            CourseRootObject courseObj = new CourseRootObject();
                            
                            string jsonCourseData = File.ReadAllText(coursespath);
                            string jsonCourse;

                            courseObj = JsonConvert.DeserializeObject<CourseRootObject>(jsonCourseData);
                            coursesList = courseObj.courses.ToList<Course>();
                            foreach (var i in itemToRemove.StudentCourses)
                            {
                                foreach (Course course in coursesList)
                                {
                                    if (course.Code == i)
                                    {
                                        course.seats = course.seats + 1;
                                        courseObj.courses = coursesList.ToArray<Course>();
                                        jsonCourse = JsonConvert.SerializeObject(courseObj, Formatting.Indented);
                                        File.WriteAllText(coursespath, jsonCourse);
                                    }
                                }
                            }
                        }

                        usersList.Remove(itemToRemove);
                        usersObj.users = usersList.ToArray<User>(); // Converts the list to a User object array
                        json = JsonConvert.SerializeObject(usersObj, Formatting.Indented); // Converts object to JSON string
                        File.WriteAllText(path, json); // Writes JSON data to the file
                        return "Student Removed successfully";
                    }
                    else
                    {
                        return "Student not found";
                    }
                    
                }
                else
                {
                    return "Student ID not found in records.";
                }
            }
            finally
            {
                
            }
            return "Student Removed successfully";
        }




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
            public string[] StudentCourses { get; set; }
        }

        public class Course
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public Int32 seats { get; set; }
        }

        public class CourseRootObject
        {
            public Course[] courses { get; set; }
        }
    }
}
