﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EM.Sample.DomainLogic;
using EM.Sample.DomainModels.Models;
using EM.Sample.DomainModels.Enums;
using EM.Sample.DomainModels.Filters;
using EM.Data.Helpers;

namespace EM.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorQueriesController : ControllerBase
    {
        private readonly IAuthorQueries _authorQueries = null;

        public AuthorQueriesController(IAuthorQueries authorQueries)
        {
            _authorQueries = authorQueries;
        }

        [HttpGet("[action]")]
        public IActionResult GetAuthors()
        {
            IEnumerable<AuthorDto> authors = _authorQueries.GetAuthors();
            return Ok(authors);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAuthor([FromQuery] int id)
        {
            AuthorDto author = _authorQueries.GetAuthor(id);
            return Ok(author);
        }
    }
}