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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DeleteCourseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DeleteCourseService.svc or DeleteCourseService.svc.cs at the Solution Explorer and start debugging.
    public class DeleteCourseService : IDeleteCourseService
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
        public Boolean deleteCourse(string Code)
        {
            //Course newCourse = new Course();
            CourseRootObject courseObj = new CourseRootObject();
            List<Course> coursesList = new List<Course>();
            string json;
            Boolean exists = false;
            Boolean created = false;

            string coursesPath = HttpRuntime.AppDomainAppPath + "\\courses_list.json";
            string usersPath = HttpRuntime.AppDomainAppPath + "\\users_list.json";

            string jsonData = File.ReadAllText(coursesPath);

            courseObj = JsonConvert.DeserializeObject<CourseRootObject>(jsonData);

            if (courseObj.courses != null)
            {
                coursesList = courseObj.courses.ToList<Course>();
                foreach (Course course in coursesList)
                {
                    if (course.Code == Code)
                    {
                        exists = true;
                    }
                }
            }

            if (exists)
            {
                var itemToRemove = coursesList.SingleOrDefault(r => r.Code == Code);
                if (itemToRemove != null)
                {

                    List<User> usersList = new List<User>();
                    UsersRootObject usersObj = new UsersRootObject(); // Object of user

                    string jsonUserData = File.ReadAllText(usersPath);
                    string jsonUser;

                    usersObj = JsonConvert.DeserializeObject<UsersRootObject>(jsonUserData);

                    usersList = usersObj.users.ToList<User>();
                    foreach (var i in itemToRemove.CourseStudents)
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

                    usersList.Remove(itemToRemove);
                    usersObj.users = usersList.ToArray<User>(); // Converts the list to a User object array
                    json = JsonConvert.SerializeObject(usersObj, Formatting.Indented); // Converts object to JSON string
                    File.WriteAllText(coursesPath, json); // Writes JSON data to the file
                    return "Student Removed successfully";
                }
                else
                {
                    return "Student not found";
                }
            }
            return created;
        }
    }
}
