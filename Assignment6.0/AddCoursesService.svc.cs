using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Web;

namespace Assignment6._0
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AddCoursesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AddCoursesService.svc or AddCoursesService.svc.cs at the Solution Explorer and start debugging.
    public class AddCoursesService: IAddCoursesService
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

        public Boolean addCourse(string Code, string Name, Int32 seats)
        {
            Course newCourse = new Course();
            CourseRootObject courseObj = new CourseRootObject();
            List<Course> coursesList = new List<Course>();
            string json;
            Boolean exists = false;
            Boolean created = false;

            string path = HttpRuntime.AppDomainAppPath + "\\courses_list.json";

            string jsonData = File.ReadAllText(path);

            courseObj = JsonConvert.DeserializeObject<CourseRootObject>(jsonData);

            if(courseObj.courses != null)
            {
                coursesList = courseObj.courses.ToList<Course>();
                foreach (Course course in coursesList)
                {
                    if(course.Code == Code)
                    {
                        exists = true;
                    }
                }
            }

            if (!exists)
            {
                newCourse.Code = Code; newCourse.Name = Name; newCourse.seats = seats;
                newCourse.CourseStudents = new List<string>();
                coursesList.Add(newCourse);

                courseObj.courses = coursesList.ToArray<Course>();
                json = JsonConvert.SerializeObject(courseObj, Formatting.Indented);
                File.WriteAllText(path, json);

                created = true;
            }
            return created;
        }
    }
}
