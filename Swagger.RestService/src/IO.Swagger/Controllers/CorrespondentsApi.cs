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
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic;
using AutoMapper;
using Paperless.DAL.Interfaces;

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class CorrespondentsApiController : ControllerBase
    {
        ICorrespondentLogic _correspondentLogic;
        IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="repository"></param>
        public CorrespondentsApiController(IMapper mapper, ICorrespondentRepository repository)
        {
            _mapper = mapper;
            _correspondentLogic = new CorrespondentLogic(repository, _mapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Success</response>
        [HttpPost]
        [Route("/api/correspondents")]
        [ValidateModelState]
        [SwaggerOperation("CreateCorrespondent")]
        [SwaggerResponse(statusCode: 200, type: typeof(ApiCorrespondentsBody), description: "Success")]
        public virtual IActionResult CreateCorrespondent([FromBody]ApiCorrespondentsBody body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(ApiCorrespondentsBody));
            string exampleJson = null;
            exampleJson = "{\n  \"owner\" : 6,\n  \"matching_algorithm\" : 0,\n  \"is_insensitive\" : true,\n  \"name\" : \"name\",\n  \"match\" : \"match\"\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<ApiCorrespondentsBody>(exampleJson)
                        : default(ApiCorrespondentsBody);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Success</response>
        [HttpDelete]
        [Route("/api/correspondents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteCorrespondent")]
        public virtual IActionResult DeleteCorrespondent([FromRoute][Required]int? id)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="fullPerms"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/correspondents")]
        [ValidateModelState]
        [SwaggerOperation("GetCorrespondents")]
        [SwaggerResponse(statusCode: 200, type: typeof(Correspondent), description: "Success")]
        public virtual IActionResult GetCorrespondents([FromQuery]int? page, [FromQuery]bool? fullPerms)
        {
            var cor = _mapper.Map<
                ICollection<Paperless.BusinessLogic.Entities.Correspondent>, 
                ICollection<Correspondent>>(
                    _correspondentLogic.GetCorrespondents());
            return Ok(cor);
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse200));
            /*
            string exampleJson = null;
            exampleJson = "{\n  \"next\" : 6,\n  \"all\" : [ 5, 5 ],\n  \"previous\" : 1,\n  \"count\" : 0,\n  \"results\" : [ {\n    \"owner\" : 9,\n    \"matching_algorithm\" : 2,\n    \"document_count\" : 7,\n    \"is_insensitive\" : true,\n    \"permissions\" : {\n      \"view\" : {\n        \"groups\" : [ \"\", \"\" ],\n        \"users\" : [ \"\", \"\" ]\n      }\n    },\n    \"name\" : \"name\",\n    \"match\" : \"match\",\n    \"id\" : 5,\n    \"last_correspondence\" : \"last_correspondence\",\n    \"slug\" : \"slug\"\n  }, {\n    \"owner\" : 9,\n    \"matching_algorithm\" : 2,\n    \"document_count\" : 7,\n    \"is_insensitive\" : true,\n    \"permissions\" : {\n      \"view\" : {\n        \"groups\" : [ \"\", \"\" ],\n        \"users\" : [ \"\", \"\" ]\n      }\n    },\n    \"name\" : \"name\",\n    \"match\" : \"match\",\n    \"id\" : 5,\n    \"last_correspondence\" : \"last_correspondence\",\n    \"slug\" : \"slug\"\n  } ]\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<InlineResponse200>(exampleJson)
                        : default(InlineResponse200);            //TODO: Change the data returned
            return new ObjectResult(example);
            */
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <response code="200">Success</response>
        [HttpPut]
        [Route("/api/correspondents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateCorrespondent")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2001), description: "Success")]
        public virtual IActionResult UpdateCorrespondent([FromRoute][Required]int? id, [FromBody]CorrespondentsIdBody body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2001));
            string exampleJson = null;
            exampleJson = "{\n  \"owner\" : 5,\n  \"matching_algorithm\" : 6,\n  \"user_can_change\" : true,\n  \"document_count\" : 1,\n  \"is_insensitive\" : true,\n  \"name\" : \"name\",\n  \"match\" : \"match\",\n  \"id\" : 0,\n  \"last_correspondence\" : 5,\n  \"slug\" : \"slug\"\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<InlineResponse2001>(exampleJson)
                        : default(InlineResponse2001);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
