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

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class DocumentsApiController : ControllerBase
    {
        private readonly IDocumentLogic documentLogic;

        public DocumentsApiController(IDocumentLogic documentLogic) 
        { 
            this.documentLogic = documentLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Success</response>
        [HttpPost]
        [Route("/api/documents")]
        [ValidateModelState]
        [SwaggerOperation("UpdateDocument")]
        public virtual IActionResult UpdateDocument()
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            throw new NotImplementedException();
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
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="original"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/documents/{id}/download")]
        [ValidateModelState]
        [SwaggerOperation("DownloadDocument")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Success")]
        public virtual IActionResult DownloadDocument([FromRoute][Required]int? id, [FromQuery]bool? original)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(byte[]));
            string exampleJson = null;
            exampleJson = "\"\"";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<byte[]>(exampleJson)
                        : default(byte[]);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="fullPerms"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/documents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("GetDocument")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2003), description: "Success")]
        public virtual IActionResult GetDocument([FromRoute][Required]int? id, [FromQuery]int? page, [FromQuery]bool? fullPerms)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2003));
            string exampleJson = null;
            exampleJson = "{\n  \"owner\" : 7,\n  \"archive_serial_number\" : 2,\n  \"notes\" : [ {\n    \"note\" : \"note\",\n    \"created\" : \"created\",\n    \"document\" : 1,\n    \"id\" : 7,\n    \"user\" : 1\n  }, {\n    \"note\" : \"note\",\n    \"created\" : \"created\",\n    \"document\" : 1,\n    \"id\" : 7,\n    \"user\" : 1\n  } ],\n  \"added\" : \"added\",\n  \"created\" : \"created\",\n  \"title\" : \"title\",\n  \"content\" : \"content\",\n  \"tags\" : [ 5, 5 ],\n  \"storage_path\" : 5,\n  \"permissions\" : {\n    \"view\" : {\n      \"groups\" : [ 3, 3 ],\n      \"users\" : [ 9, 9 ]\n    }\n  },\n  \"archived_file_name\" : \"archived_file_name\",\n  \"modified\" : \"modified\",\n  \"correspondent\" : 6,\n  \"original_file_name\" : \"original_file_name\",\n  \"id\" : 0,\n  \"created_date\" : \"created_date\",\n  \"document_type\" : 1\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<InlineResponse2003>(exampleJson)
                        : default(InlineResponse2003);            //TODO: Change the data returned
            return new ObjectResult(example);
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
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2007));
            string exampleJson = null;
            exampleJson = "{\n  \"archive_size\" : 6,\n  \"archive_metadata\" : [ {\n    \"prefix\" : \"prefix\",\n    \"namespace\" : \"namespace\",\n    \"value\" : \"value\",\n    \"key\" : \"key\"\n  }, {\n    \"prefix\" : \"prefix\",\n    \"namespace\" : \"namespace\",\n    \"value\" : \"value\",\n    \"key\" : \"key\"\n  } ],\n  \"original_metadata\" : [ \"\", \"\" ],\n  \"original_filename\" : \"original_filename\",\n  \"original_mime_type\" : \"original_mime_type\",\n  \"archive_checksum\" : \"archive_checksum\",\n  \"original_checksum\" : \"original_checksum\",\n  \"lang\" : \"lang\",\n  \"media_filename\" : \"media_filename\",\n  \"has_archive_version\" : true,\n  \"archive_media_filename\" : \"archive_media_filename\",\n  \"original_size\" : 0\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<InlineResponse2007>(exampleJson)
                        : default(InlineResponse2007);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/documents")]
        [ValidateModelState]
        [SwaggerOperation("GetDocuments")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2002), description: "Success")]
        public virtual IActionResult GetDocuments()
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2002));
            string exampleJson = null;
            exampleJson = "{\n  \"next\" : 6,\n  \"all\" : [ 5, 5 ],\n  \"previous\" : 1,\n  \"count\" : 0,\n  \"results\" : [ {\n    \"owner\" : 4,\n    \"user_can_change\" : true,\n    \"archive_serial_number\" : 2,\n    \"notes\" : [ {\n      \"note\" : \"note\",\n      \"created\" : \"created\",\n      \"document\" : 1,\n      \"id\" : 7,\n      \"user\" : 1\n    }, {\n      \"note\" : \"note\",\n      \"created\" : \"created\",\n      \"document\" : 1,\n      \"id\" : 7,\n      \"user\" : 1\n    } ],\n    \"added\" : \"added\",\n    \"created\" : \"created\",\n    \"title\" : \"title\",\n    \"content\" : \"content\",\n    \"tags\" : [ 3, 3 ],\n    \"storage_path\" : 9,\n    \"archived_file_name\" : \"archived_file_name\",\n    \"modified\" : \"modified\",\n    \"correspondent\" : 2,\n    \"original_file_name\" : \"original_file_name\",\n    \"id\" : 5,\n    \"created_date\" : \"created_date\",\n    \"document_type\" : 7\n  }, {\n    \"owner\" : 4,\n    \"user_can_change\" : true,\n    \"archive_serial_number\" : 2,\n    \"notes\" : [ {\n      \"note\" : \"note\",\n      \"created\" : \"created\",\n      \"document\" : 1,\n      \"id\" : 7,\n      \"user\" : 1\n    }, {\n      \"note\" : \"note\",\n      \"created\" : \"created\",\n      \"document\" : 1,\n      \"id\" : 7,\n      \"user\" : 1\n    } ],\n    \"added\" : \"added\",\n    \"created\" : \"created\",\n    \"title\" : \"title\",\n    \"content\" : \"content\",\n    \"tags\" : [ 3, 3 ],\n    \"storage_path\" : 9,\n    \"archived_file_name\" : \"archived_file_name\",\n    \"modified\" : \"modified\",\n    \"correspondent\" : 2,\n    \"original_file_name\" : \"original_file_name\",\n    \"id\" : 5,\n    \"created_date\" : \"created_date\",\n    \"document_type\" : 7\n  } ]\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<InlineResponse2002>(exampleJson)
                        : default(InlineResponse2002);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        [HttpPut]
        [Route("/api/documents/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateDocument")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2004), description: "Success")]
        public virtual IActionResult UpdateDocument([FromRoute][Required]int? id)//, [FromBody]DocumentsIdBody body)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2004));
            //string exampleJson = null;
            //exampleJson = "{\n  \"owner\" : 7,\n  \"user_can_change\" : true,\n  \"archive_serial_number\" : 2,\n  \"notes\" : [ \"\", \"\" ],\n  \"added\" : \"added\",\n  \"created\" : \"created\",\n  \"title\" : \"title\",\n  \"content\" : \"content\",\n  \"tags\" : [ 5, 5 ],\n  \"storage_path\" : 5,\n  \"archived_file_name\" : \"archived_file_name\",\n  \"modified\" : \"modified\",\n  \"correspondent\" : 6,\n  \"original_file_name\" : \"original_file_name\",\n  \"id\" : 0,\n  \"created_date\" : \"created_date\",\n  \"document_type\" : 1\n}";
            //            var example = exampleJson != null
            //            ? JsonConvert.DeserializeObject<InlineResponse2004>(exampleJson)
            //            : default(InlineResponse2004);            //TODO: Change the data returned

            documentLogic.PublishOCRJob(new Paperless.BusinessLogic.Entities.Document
                    {
                        Id = 1,
                        Correspondent = 123,
                        DocumentType = 456,
                        Title = "Sample Document",
                        Content = "This is a sample content",
                        Created = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        Modified = DateTime.Now,
                        Added = DateTime.Now,
                        OriginalFileName = "sample.txt",
                        ArchivedFileName = "archived_sample.txt"
                    });

            return Ok();
        }

    }
}
