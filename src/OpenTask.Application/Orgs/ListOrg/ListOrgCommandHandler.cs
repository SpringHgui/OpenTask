using OpenTask.Application.Base.Commands;
using OpenTask.Application.Base;
using OpenTask.Domain.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Orgs.ListOrg
{
    public class ListOrgCommandHandler : ICommandHandler<ListOrgCommand, ListOrgResponse>
    {
        IOrganizationRepository orgRepository;

        public ListOrgCommandHandler(IOrganizationRepository orgRepository)
        {
            this.orgRepository = orgRepository;
        }

        public Task<ListOrgResponse> Handle(ListOrgCommand request, CancellationToken cancellationToken)
        {
            var orgs = orgRepository.ListOrgForCurrentUser(request.UserId) ?? Enumerable.Empty<Organization>();

            return Task.FromResult(new ListOrgResponse
            {
                Orgs = orgs
            });
        }
    }
}
