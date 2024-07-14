using OpenTask.Application.Base.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Members.ListMembers
{
    public class ListMembersRequest : ICommand<ListMembersResponse>
    {
        public long OrganizationId { get; set; }
    }
}
