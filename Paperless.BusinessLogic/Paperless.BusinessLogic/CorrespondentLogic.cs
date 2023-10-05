using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.BusinessLogic
{
    public class CorrespondentLogic : ICorrespondentLogic
    {
        public CorrespondentLogic() { }

        public Correspondent GetCorrespondent()
        {
            //Fetch correspondents from DB
            //return results
            //List<Correspondent> result = new List<Correspondent>();
            Correspondent correspondent = new Correspondent();
            correspondent.Name = "Test";
            correspondent.Id = 1;
            correspondent.MatchingAlgorithm = 2;
            correspondent.IsInsensitive = true;
            correspondent.DocumentCount = 1;
            correspondent.LastCorrespondence = DateTime.Now;
            
            //result.Add(new Correspondent());
            return correspondent;
        }
    }
}
