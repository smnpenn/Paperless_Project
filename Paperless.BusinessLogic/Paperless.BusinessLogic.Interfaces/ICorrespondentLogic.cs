﻿namespace Paperless.BusinessLogic.Interfaces
{
    public interface ICorrespondentLogic
    {
        public Entities.Correspondent GetCorrespondent(long id);
        public ICollection<Entities.Correspondent> GetCorrespondents();
    }
}
