using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace PeoplesPortalNUnitTest
{
    public class LoginPageTest
    {
        private AccountDbAccess dbAccess { get; set; } = null!;
        [SetUp]
        public void Setup()
        {
            PeoplesPortalNUnitTest.DbAccess.DbConnection dbConnection = new PeoplesPortalNUnitTest.DbAccess.DbConnection();
            // Arrange
            dbAccess = new AccountDbAccess(dbConnection.Configuration); // Instantiate your Razor Page model class
        }

        [Test]
        public void LoginTestWithIncorrectUserAndPassword()
        {
            AccountModel account = new AccountModel();
            account.UserName = "ABCD";
            account.Password = "123";
            ResponseMessage response = dbAccess.AuthenticateUser(account);

            //Assertions
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == System.Net.HttpStatusCode.OK);
            Assert.AreSame(response.Message == "Login successful",null);
            
        }

        [Test]
        public void LoginTestWithCorrectUserAndPassword()
        {
            AccountModel account = new AccountModel();
            account.UserName = "Admin";
            account.Password = "admin";
            ResponseMessage response = dbAccess.AuthenticateUser(account);

            //Assertions
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == System.Net.HttpStatusCode.OK);
            Assert.AreSame(response.Message == "Login successful", null);

        }
    }
}
