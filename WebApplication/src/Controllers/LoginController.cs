using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace WebApplication.Controllers
{
    [Route("api")]
    public class LoginController : ControllerBase
    {
        [Route("sign-in")]
        public async Task Login(string userName)
        {
            Response.StatusCode = 401;
        }
    }
}