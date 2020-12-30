using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    [Route("user")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        public async Task<string> Hello()
        {
            return User.Identity.Name;
        }
    }
}