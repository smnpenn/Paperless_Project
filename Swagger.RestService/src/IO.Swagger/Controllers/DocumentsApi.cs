/*
 * Paperless Rest Server
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using IO.Swagger.Attributes;

using Microsoft.AspNetCore.Authorization;
using IO.Swagger.Models;
using Paperless.BusinessLogic;
using Paperless.BusinessLogic.Interfaces;
using AutoMapper;
using Paperless.DAL.Interfaces;
using Paperless.ServiceAgents.Interfaces;
using System.IO;
using Microsoft.Extensions.Logging;

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class DocumentsApiController : ControllerBase
    {
        ILogger _log;
        private readonly IDocumentLogic documentLogic;
        private IMapper _mapper;
        private DocumentValidator validator;
        private IElasticSearchServiceAgent elasticSearchServiceAgent;

        /// <summary>
        /// Documents API Controller
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="rabbitMQService"></param>
        /// <param name="minIOService"></param>
        /// <param name="elasticSearchServiceAgent"></param>

        public DocumentsApiController(ILogger<DocumentsApiController> logger, IDocumentLogic documentLogic, IMapper mapper) 
        { 
            _log = logger;
            this.documentLogic = documentLogic;
            _mapper = mapper;
            validator = new DocumentValidator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">Success</response>
        [HttpPost]
        [Route("/api/documents")]
        [ValidateModelState]
        [SwaggerOperation("UploadDocument")]
        public virtual IActionResult UploadDocument()
        {
            Document newDocument = null;
            Stream newDocumentFile = null;

            try
            {
                var newDocumentTags = new List<int?>();

                foreach (var tag in HttpContext.Request.Form["tags"])
                {
                    newDocumentTags.Add(int.Parse(tag));
                }

                var newDocumentDate = DateTime.Parse(HttpContext.Request.Form["created"]);
                newDocumentDate = DateTime.SpecifyKind(newDocumentDate, DateTimeKind.Utc);

                newDocument = new Document
                {
                    Title = HttpContext.Request.Form["title"],
                    Created = newDocumentDate,
                    Modified = newDocumentDate,
                    Added = newDocumentDate,
                    DocumentType = int.Parse(HttpContext.Request.Form["document_type"]),
                    Tags = newDocumentTags,
                    Correspondent = int.Parse(HttpContext.Request.Form["correspondent"])
                };

                newDocumentFile = HttpContext.Request.Form.Files["file1"].OpenReadStream();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return BadRequest();
            }

            _log.LogInformation("trying to upload document");

            try
            {
                documentLogic.SaveDocument(
                    _mapper.Map<Paperless.BusinessLogic.Entities.Document>(newDocument),
                    newDocumentFile);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Success</response>
        [HttpDelete]
        [Route("/api/documents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteDocument")]
        public virtual IActionResult DeleteDocument([FromRoute][Required]int? id)
        {
            try
            {
                if (documentLogic.DeleteDocument((Int64)id) == false) return BadRequest();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/documents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("GetDocument")]
        [SwaggerResponse(statusCode: 200, type: typeof(Document), description: "Success")]
        public virtual IActionResult GetDocument([FromRoute][Required]Int64 id)
        {
            Document res = _mapper.Map<Paperless.BusinessLogic.Entities.Document, Document>(documentLogic.GetDocumentById(id));

            if(res == null)
                return BadRequest();
            else
                return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/documents/{id}/metadata")]
        [ValidateModelState]
        [SwaggerOperation("GetDocumentMetadata")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2007), description: "Success")]
        public virtual IActionResult GetDocumentMetadata([FromRoute][Required]int? id)
        { 
            string res = documentLogic.GetDocumentMetadata((Int64)id);

            if(res == null)
                return BadRequest();
            else
                return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/documents")]
        [ValidateModelState]
        [SwaggerOperation("GetDocuments")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Document>), description: "Success")]
        public virtual IActionResult GetDocuments()
        {
            ICollection<Document> result = null;

            try
            {
                result = _mapper.Map<ICollection<Paperless.BusinessLogic.Entities.Document>, ICollection<Document>>(documentLogic.GetDocuments());

                if (result.Count == 0) return NoContent();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <response code="200">Success</response>
        [HttpPut]
        [Route("/api/documents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateDocument")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2004), description: "Success")]
        public virtual IActionResult UpdateDocument([FromRoute][Required]int? id, [FromBody] Document body)
        {
            try
            {
                documentLogic.UpdateDocument((Int64)id, _mapper.Map<Paperless.BusinessLogic.Entities.Document>(body));
            }
            catch (Exception ex) 
            {
                _log.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok();
        }

    }
}
