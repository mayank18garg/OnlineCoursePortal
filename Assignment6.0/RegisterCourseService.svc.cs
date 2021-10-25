using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Web;

namespace Assignment6._0
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegisterCourseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RegisterCourseService.svc or RegisterCourseService.svc.cs at the Solution Explorer and start debugging.
    public class RegisterCourseService : IRegisterCourseService
    {
        public class Course
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public Int32 seats { get; set; }
            public List<string> CourseStudents { get; set; }
        }

        public class CourseRootObject
        {
            public Course[] courses { get; set; }
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
            public List<string> StudentCourses { get; set; }
        }
        public string Registercourse(string courseCode, string userId)
        {
            List<Course> coursesList = new List<Course>();
            CourseRootObject courseObj = new CourseRootObject();
            string path = HttpRuntime.AppDomainAppPath + "\\courses_list.json";

            //user
            string userpath = HttpRuntime.AppDomainAppPath + "\\users_list.json"; // File path to user credentials 
            string jsonUserData = File.ReadAllText(userpath); // reads in the JSON file into a string

            User newUser = new User(); // User object for new user
            UsersRootObject usersObj = new UsersRootObject(); // Object of user
            List<User> usersList = new List<User>(); // List of users to read in existing data and add new users

            usersObj = JsonConvert.DeserializeObject<UsersRootObject>(jsonUserData); // transfers jsonData to the usersObj
            usersList = usersObj.users.ToList<User>();
            //user
            string jsonData = File.ReadAllText(path);
            string json;

            courseObj = JsonConvert.DeserializeObject<CourseRootObject>(jsonData);
            coursesList = courseObj.courses.ToList<Course>();
            foreach (Course course in coursesList)
            {
                if (course.Code == courseCode)
                {
                    course.seats = course.seats - 1;
                    course.CourseStudents.Add(userId);
                    courseObj.courses = coursesList.ToArray<Course>();
                    json = JsonConvert.SerializeObject(courseObj, Formatting.Indented);
                    File.WriteAllText(path, json);

                    var itemToAdd = usersList.SingleOrDefault(r => r.StudentID == userId);
                    if (itemToAdd != null)
                    {
                        itemToAdd.StudentCourses.Add(course.Code);
                    }
                    
                }
               
            }
            usersObj.users = usersList.ToArray<User>(); // Converts the list to a User object array
            json = JsonConvert.SerializeObject(usersObj, Formatting.Indented); // Converts object to JSON string
            File.WriteAllText(userpath, json); // Writes JSON data to the file

            string ans = string.Format("Course {0} has been registered for user {1}", courseCode, userId);
            return ans;
        }
    }
}
