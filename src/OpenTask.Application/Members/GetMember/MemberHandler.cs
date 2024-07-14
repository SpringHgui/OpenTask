using MediatR;
using OpenTask.Application.Base.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Members.GetMember
{
    public class MemberHandler : ICommandHandler<MemberRequest, MemberResponse>
    {
        public Task<MemberResponse> Handle(MemberRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
