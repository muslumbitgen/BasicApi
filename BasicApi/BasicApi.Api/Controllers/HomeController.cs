using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicApi.Core.Services;
using BasicApi.Items.Dtos;
using BasicApi.Items.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicApi.Api.Controllers
{
    [Route("api/basicapi/v1/home"), AllowAnonymous]
    public class HomeController : BaseController
    {
        public HomeController()
        {
           
        }
    }
}