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
        public string Registercourse(string courseCode, string userId)
        {
            List<Course> coursesList = new List<Course>();
            CourseRootObject courseObj = new CourseRootObject();
            string path = HttpRuntime.AppDomainAppPath + "\\courses_list.json";
            string jsonData = File.ReadAllText(path);
            string json;

            courseObj = JsonConvert.DeserializeObject<CourseRootObject>(jsonData);
            coursesList = courseObj.courses.ToList<Course>();
            foreach (Course course in coursesList)
            {
                if (course.Code == courseCode)
                {
                    course.seats = course.seats - 1;
                    courseObj.courses = coursesList.ToArray<Course>();
                    json = JsonConvert.SerializeObject(courseObj, Formatting.Indented);
                    File.WriteAllText(path, json);
                }
            }
            string ans = string.Format("Course {0} has been registered for user {1}",courseCode,userName);
            return ans;
        }
    }
}
