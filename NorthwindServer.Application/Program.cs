
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;

namespace NorthwindServer.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //Nancy Framework : 

            /* 
             * IoT dünyasına dahil olan cihazlar birbirleri ile haberleşmek için hafif donatılmış servislerden yararlanabilirler. 
             * Bu tip servislerin kolayca geliştirilebilmesi için pek çok Framework söz konusu. Nancy bu çatılardan sadece bir tanesi. 
             * Nancy Framework ile REST tabanlı servislerin kolay bir şekilde geliştirilmesi mümkün. 
             * Kendisi aynı zamanda bir Service Framework olarak da düşünülebilir. 
             * Bu yüzden kendi başına host edilebilen bir servis motoru da içermektedir. 
             * Hatta WCF, OWIN ve Asp.Net MVC üzerinde de host edilbilen bir yapıya sahiptir. 
             * IoC(Inversion of Control) ilkesine göre geliştirilmiş modüler bir yapıya sahip. 
             * Bu yüzden plug-in stilinde servis içeriğinin genişletilmesi son derece kolay. 
             * IoC sayesinde çalışma zamanına modüller zahmetsizce bağlanabilmekte. 
             * Karmaşık Route tanımlamalarının basitçe yapılmasına izin veriyor. 
             * JSON(JavaScript Object Notation) formatıyla oldukça dostane bir ilişkisi var(Elbette XML desteği de bulunuyor ama dünyadaki trend JSON yönünde). 


             * Ancak tüm bunlar bir yana belki de en önemli özelliği, üretilen exe ve dll' lerin Mono destekli Linux ortamlarına atıldıktan sonra da sorunsuz çalışıyor olması
             * Dolayısıyla Windows sistemi üzerinde yazılan REST servislerini, Linux platformuna taşıyarak çalıştırmak da son derece kolay.
             * IoT denince ilk akla gelen Arduino ve Raspberry PI sistemleri ile de sorunsuz bir şekilde çalıştığı ifade ediliyor
             */

            Uri hostUrl = new Uri("http://127.0.0.1:5555");

            using (NancyHost server = new NancyHost(hostUrl))
            {

                server.Start();

                Console.WriteLine("Nancy {0} adresinden dinlemede!", hostUrl.ToString());

                Console.WriteLine("Kapatmak için bir tuşa basın...");

                Console.ReadLine();

                server.Stop();

            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public EmployeeDepartment EmployeeDepartment { get; set; }
    }

    public enum EmployeeDepartment
    {
        IT = 1, Sales = 2, Management = 3
    }

    public static class EmployeeDataHelper
    {
        public static IList<Employee> List()
        {
            return new List<Employee>()
            {
                new Employee(){Id = 1, Name = "Alexis", Surname = "Texas", EmployeeDepartment =      EmployeeDepartment.IT},
                new Employee(){Id = 2, Name = "Julia", Surname = "Anderson", EmployeeDepartment =      EmployeeDepartment.IT}, 
                new Employee(){Id = 3, Name = "Ali Suleyman", Surname = "Topuz", EmployeeDepartment =      EmployeeDepartment.IT},
                new Employee(){Id = 4, Name = "Vladimir", Surname = "Neptun", EmployeeDepartment =      EmployeeDepartment.Sales},
                new Employee(){Id = 5, Name = "Sahin", Surname = "Yılmaz", EmployeeDepartment =      EmployeeDepartment.Management},
                new Employee(){Id = 6, Name = "Julia", Surname = "Alexandratou", EmployeeDepartment =      EmployeeDepartment.Management},
            };
        }
    }

    public class ApplicationRoutes : NancyModule
    {
        public ApplicationRoutes()
        {
            Get["/"] = p =>
            {

                return "<b><h1>Northwind Data Server</h1></b>";

            };

            Get["employees"] = AllEmployees;

            Get["employee/{ID}"] = FindByID;

            //Post["add"] = AddEmployee;
        }

        dynamic FindByID(dynamic parameters)
        {

            var employee = EmployeeDataHelper.List().FirstOrDefault(p => p.Id == parameters.ID);

            return Response.AsXml(employee);

        }

        dynamic AllEmployees(dynamic parameters)
        {

            return Response.AsJson(EmployeeDataHelper.List());

        }

        //dynamic AddEmployee(dynamic parameters)
        //{

        //    Employee emp = this.Bind<Employee>();

        //    employees.Add(emp);



        //    return string.Format("{0}[{1} - {2}] eklendi"

        //        , emp.Id  

        //        , emp.Name

        //        , emp.Surname);

        //}
    }
}
