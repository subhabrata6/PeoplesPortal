using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;
using TestEmployeeApp.Pages;
using TestEmployeeApp.Pages.Peoples;

namespace PeoplesPortalNUnitTest
{
    public class PeoplePageTest
    {
        private PeopleDbAccess dbAccess { get; set; } = null!;


        [SetUp]
        public void Setup()
        {
            PeoplesPortalNUnitTest.DbAccess.DbConnection dbConnection = new PeoplesPortalNUnitTest.DbAccess.DbConnection();
            // Arrange
            dbAccess = new PeopleDbAccess(dbConnection.Configuration); // Instantiate your Razor Page model class
        }

        [Test]
        public void PeoplePageOnGetTest()
        {


            var response = dbAccess.GetPeopleList();
            TestDelegate @delegate = () => dbAccess.GetPeopleList();

            // Assert
            Assert.IsNotNull(response); // Ensure response is not null
            Assert.True(response.Status == System.Net.HttpStatusCode.OK); // Ensure response status is ok
            Assert.DoesNotThrow(@delegate); // Ensure that the api call doesn't throw an exception

            // Arrange
            List<People> myList = (List<People>)response.Response;

            // Act & Assert
            Assert.IsTrue(myList.TrueForAll(obj => !string.IsNullOrEmpty(obj.name)), "At least one object has a null or empty Name property");
            Assert.IsTrue(myList.TrueForAll(obj => !string.IsNullOrEmpty(obj.createdOn)), "At least one object has a null or empty Created on property");
            Assert.IsTrue(myList.TrueForAll(obj => obj.age <= 0), "At least one object has a missing age property");
            Assert.IsTrue(myList.TrueForAll(obj => obj.countryId <= 0), "At least one object has a missing country property");
            Assert.IsTrue(myList.TrueForAll(obj => obj.stateId <= 0), "At least one object has a missing state property");

            // Add more assertions based on your page logic
            //Assert.Pass();
        }

        [Test]
        public void PeoplePageOnPostTest()
        {
            /*
            //For new insertion
            People myPeople = new People();
            myPeople.id = 0;
            myPeople.name = "New Tester";
            myPeople.description = "Tester";
            myPeople.countryId = 1;
            myPeople.stateId = 1;
            myPeople.age = 20;

            var response = dbAccess.SavePeople(myPeople);

            TestDelegate @delegate = () => dbAccess.SavePeople(myPeople);

            // Assert
            Assert.IsNotNull(response); // Ensure response is not null
            Assert.True(response.Status == System.Net.HttpStatusCode.OK); // Ensure response status is ok
            Assert.DoesNotThrow(@delegate); // Ensure that the api call doesn't throw an exception

            */

            // For updation
            People myPeople = new People();
            myPeople.id = 14;
            myPeople.name = "Subhabrata Saha";
            myPeople.description = "Software Developer";
            myPeople.countryId = 1;
            myPeople.stateId = 8;
            myPeople.age = 20;

            var updateResponse = dbAccess.SavePeople(myPeople);

            TestDelegate @updateDelegate = () => dbAccess.SavePeople(myPeople);

            // Assert
            Assert.IsNotNull(updateResponse); // Ensure response is not null
            Assert.True(updateResponse.Status == System.Net.HttpStatusCode.OK); // Ensure response status is ok
            Assert.DoesNotThrow(@updateDelegate); // Ensure that the api call doesn't throw an exception
        }
    }
}
