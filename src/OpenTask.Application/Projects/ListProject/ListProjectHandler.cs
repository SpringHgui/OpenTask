using OpenTask.Application.Base.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Projects.ListProject
{
    public class ListProjectHandler : IQueryHandler<ListProjectRequest, ListProjectResponse>
    {
        public Task<ListProjectResponse> Handle(ListProjectRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
