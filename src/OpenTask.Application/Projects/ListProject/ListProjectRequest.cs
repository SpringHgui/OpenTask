using OpenTask.Application.Base.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Projects.ListProject
{
    public class ListProjectRequest : IQuery<ListProjectResponse>
    {
        public long OrganizationId { get; set; }
    }
}
