﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SikkimGov.Platform.DataAccess.Repositories.Contracts;
using SikkimGov.Platform.Models.DomainModels;

namespace SikkimGov.Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DDOController : ControllerBase
    {
        private readonly IDDORepository ddoRepository;
        public DDOController(IDDORepository ddoRepository)
        {
            this.ddoRepository = ddoRepository;
        }

        [Route("search")]
        [HttpGet]
        public List<DDOBase> Get([FromQuery] int deptId)
        {
            return this.ddoRepository.GetDDOBaseByDeparmentId(deptId);
        }

        [Route("details")]
        [HttpGet]
        public DDODetails GetDDODetails([FromQuery]string ddoCode)
        {
            return this.ddoRepository.GetDDODetailsByDDOCode(ddoCode);
        }
    }
}
