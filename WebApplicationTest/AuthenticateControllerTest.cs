using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplicationTest
{
    public class AuthenticateControllerTest
    {
        [Test]
        public async Task Setup()
        {
            var mockDb = new Mock<Db>();

            mockDb.Setup(x => x.GetAccountAsync(It.IsAny<Guid>())).ReturnsAsync(new Account(){Name = "sasi"});

            var account =  await mockDb.Object.GetAccountAsync(new Guid());
            
            Assert.AreEqual("sasi",account.Name);
        }
    }
}