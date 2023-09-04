using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace HelpCheck_API.Controllers.WebApps
{
    // [EnableCors("CorsAPI")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Website Application")]
    [Route("web-apps/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[AllowAnonymous]
    public class BaseController : ControllerBase
    {
        private static readonly AppSettingHelper appSettingHelper = new AppSettingHelper();

        //protected readonly ILogger<BaseController> _logger;

        //public BaseController(ILogger<BaseController> logger)
        //{
        //    _logger = logger;
        //}

        //protected void BeginServiceLog()
        //{
        //    _logger.LogInformation("Start" + ControllerContext.ActionDescriptor.ControllerName + " " + ControllerContext.ActionDescriptor.ActionName + " at {date}", DateTime.Now);
        //}

        //protected void EndServiceLog(object data)
        //{
        //    _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + " " + ControllerContext.ActionDescriptor.ActionName + " result {_data}", data);
        //}

        protected string GetAccessTokenFromHeader()
        {
            string jwt = Request.Headers[HeaderNames.Authorization];
            if (jwt == null)
            {
                throw new ArgumentException("Invalid token to access.");
            }
            string[] tokens = jwt.Split(' ');
            if (tokens[0].ToLower() != Constant.TOKEN_TYPE_BEARER)
            {
                return "";
            }
            string accessTokenString = tokens[1];
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenString);

            return token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
    }
}
