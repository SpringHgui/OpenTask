using OpenTask.Application.Base.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Members.GetMember
{
    public class MemberRequest : ICommand<MemberResponse>
    {
        public long AccountId { get; set; }

        public long OrganizationId { get; set; }
    }
}
