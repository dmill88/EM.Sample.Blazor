using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using EM.Sample.DomainLogic;
using EM.Sample.DomainModels.Models;
using EM.Sample.DomainModels.Enums;
using EM.Sample.DomainModels.Filters;
using EM.Data.Helpers;

namespace EM.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<BlogPostsController> _logger;

        public AuthorController(ILogger<BlogPostsController> logger)
        {
            _logger = logger;
        }
    }


}