using OpenTask.Application.Base.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Orgs.ListOrg
{
    public class ListOrgCommand : ICommand<ListOrgResponse>
    {
        public long UserId { get; set; }
    }
}
