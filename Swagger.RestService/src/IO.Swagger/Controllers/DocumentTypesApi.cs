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
using Paperless.DAL.Interfaces;
using Paperless.BusinessLogic.Interfaces;
using AutoMapper;
using Paperless.BusinessLogic;

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class DocumentTypesApiController : ControllerBase
    {
        IDocumentTypeLogic _typeLogic;
        IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeLogic"></param>
        /// <param name="mapper"></param>
        public DocumentTypesApiController(IDocumentTypeLogic typeLogic, IMapper mapper)
        {
            _typeLogic = typeLogic;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Success</response>
        [HttpPost]
        [Route("/api/document_types")]
        [ValidateModelState]
        [SwaggerOperation("CreateDocumentType")]
        public virtual IActionResult CreateDocumentType([FromBody]DocumentType body)
        {
            int res = _typeLogic.CreateType(_mapper.Map<Paperless.BusinessLogic.Entities.DocumentType>(body));

            if (res == 0)
                return Ok();
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Success</response>
        [HttpDelete]
        [Route("/api/document_types/{id}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteDocumentType")]
        public virtual IActionResult DeleteDocumentType([FromRoute][Required]int? id)
        {
            int res = _typeLogic.DeleteType((Int64)id);

            if (res == 0)
                return Ok();
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/document_types")]
        [ValidateModelState]
        [SwaggerOperation("GetDocumentTypes")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<DocumentType>), description: "Success")]
        public virtual IActionResult GetDocumentTypes()
        {
            var res = _mapper.Map<ICollection<Paperless.BusinessLogic.Entities.DocumentType>, ICollection<DocumentType>>(_typeLogic.GetTypes());
            if (res.Count <= 0)
                return NoContent();
            else
                return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <response code="200">Success</response>
        [HttpPut]
        [Route("/api/document_types/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateDocumentType")]
        public virtual IActionResult UpdateDocumentType([FromRoute][Required]int? id, [FromBody]DocumentType body)
        {
            int res = _typeLogic.UpdateType((Int64)id, _mapper.Map<Paperless.BusinessLogic.Entities.DocumentType>(body));

            if (res == 0)
                return Ok();
            else
                return BadRequest();
        }
    }
}
