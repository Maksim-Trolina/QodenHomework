using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Database;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    [Route("user")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private Db db;

        public UserController(Db db)
        {
            this.db = db;
        }
        public async Task Transfer()
        {
            
        }
    }
}