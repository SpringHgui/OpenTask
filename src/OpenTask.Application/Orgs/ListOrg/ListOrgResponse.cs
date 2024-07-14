using OpenTask.Domain.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Orgs.ListOrg
{
    public class ListOrgResponse
    {
        public required IEnumerable<Organization> Orgs { get; set; }
    }
}
