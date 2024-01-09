using AutoMapper;
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
            CreateMap<Paperless.BusinessLogic.Entities.Correspondent, Correspondent>();
            CreateMap<Paperless.BusinessLogic.Entities.Correspondent, NewCorrespondent>();
            CreateMap<Paperless.BusinessLogic.Entities.Document, Document>();
            CreateMap<Paperless.BusinessLogic.Entities.DocumentType, DocumentType>();
            CreateMap<Paperless.BusinessLogic.Entities.DocumentType, NewDocumentType>();
            CreateMap<Paperless.BusinessLogic.Entities.UserInfo, UserInfo>();
            CreateMap<Paperless.BusinessLogic.Entities.DocTag, DocTag>();

            CreateMap<Paperless.DAL.Entities.Correspondent, Paperless.BusinessLogic.Entities.Correspondent>();
        }
    }
}
