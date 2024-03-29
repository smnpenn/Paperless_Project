﻿using AutoMapper;
using IO.Swagger.Models;

namespace IO.Swagger
{
    /// <summary>
    /// AutoMapperProfile
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoMapperProfile() 
        {
            CreateMap<Correspondent, Paperless.BusinessLogic.Entities.Correspondent>();
            CreateMap<DocTag, Paperless.BusinessLogic.Entities.DocTag>();

            CreateMap<Paperless.BusinessLogic.Entities.Correspondent, Correspondent>();
            CreateMap<Paperless.BusinessLogic.Entities.Correspondent, NewCorrespondent>();
            CreateMap<Paperless.BusinessLogic.Entities.Document, Document>();
            CreateMap<Paperless.BusinessLogic.Entities.DocumentType, DocumentType>();
            CreateMap<Paperless.BusinessLogic.Entities.DocumentType, NewDocumentType>();
            CreateMap<Paperless.BusinessLogic.Entities.UserInfo, UserInfo>();
            CreateMap<Paperless.BusinessLogic.Entities.DocTag, DocTag>();

            CreateMap<Paperless.BusinessLogic.Entities.Document, Paperless.DAL.Entities.Document>();
            CreateMap<Paperless.DAL.Entities.Correspondent, Paperless.BusinessLogic.Entities.Correspondent>();
            CreateMap<Paperless.DAL.Entities.Document, Paperless.BusinessLogic.Entities.Document>();

            CreateMap<Document, Paperless.BusinessLogic.Entities.Document>();
            CreateMap<Paperless.BusinessLogic.Entities.DocTag, Paperless.DAL.Entities.DocTag>();
            CreateMap<Paperless.BusinessLogic.Entities.Correspondent, Paperless.DAL.Entities.Correspondent>();
            CreateMap<Paperless.BusinessLogic.Entities.DocumentType, Paperless.DAL.Entities.DocumentType>();

            CreateMap<Paperless.DAL.Entities.Correspondent, Paperless.BusinessLogic.Entities.Correspondent>();
            CreateMap<Paperless.DAL.Entities.DocTag, Paperless.BusinessLogic.Entities.DocTag>();
            CreateMap<Paperless.DAL.Entities.DocumentType, Paperless.BusinessLogic.Entities.DocumentType>();
            CreateMap<DocumentType, Paperless.BusinessLogic.Entities.DocumentType>();
        }
    }
}
