﻿using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SikkimGov.Platform.Business.Services.Contracts;
using SikkimGov.Platform.Models.ApiModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SikkimGov.Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SBSFileController : ControllerBase
    {
        private readonly ISBSFileService fileService;
        private readonly ILogger<SBSFileController> logger;

        public SBSFileController(ISBSFileService fileService, ILogger<SBSFileController> logger)
        {
            this.fileService = fileService;
            this.logger = logger;
        }

        // POST api/<SBSFile>
        [HttpPost("upload")]
        public void Post([FromForm]SBSFileUploadModel model)
        {
            string targetFilePath = string.Empty;

            try
            {
                targetFilePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar.ToString()
                                + "TempFiles" + Path.DirectorySeparatorChar.ToString()
                                + "SBSFiles/" + model.File.FileName;
                using (var stream = new FileStream(targetFilePath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }

                var sbsFileType = (SBSFileType)Enum.Parse(typeof(SBSFileType), model.FileType, true);

                this.fileService.ProcessSBSFile(sbsFileType, targetFilePath);

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error while uploading SBS File.");
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                if(System.IO.File.Exists(targetFilePath))
                {
                    System.IO.File.Delete(targetFilePath);
                }
            }                            
        }
    }
}