using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;


namespace EmployeePayrollTest
{
    [TestClass]
    public class UnitTest1
    {
        //Initializing the restclient as null
        RestClient client = null;
        [TestInitialize]
        //This method is calling evrytime to initialzie the restclient object
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// UC1-->Getting all the employees from json 
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetAllEmployees()
        {
            //Get request 
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Passing the request and execute 
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        /// <summary>
        /// Testmethod to pass the test case
        /// </summary>
        [TestMethod]
        public void OnCallingRestAPI_ReturnEmployees()
        {
            IRestResponse response = GetAllEmployees();
            //Convert the json object to list(deserialize)
            var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(6, res.Count);
            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //printing the data in console
            foreach (var i in res)
            {
                Console.WriteLine("Id: {0}\t Name: {1}\t Salary :{2} ", i.id, i.name, i.salary);
            }
        }

        /// <summary>
        /// UC2--->Adding a employee in json server
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployee()
        {
            //Passing the method type as post(add details)
            RestRequest request = new RestRequest("/employees", Method.POST);
            //Creating a object
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("name", "Peter");
            json.Add("salary", 89000);
            //passing the type as json 
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //convert the jsonobject to employee object
            var res = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Peter", res.name);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
