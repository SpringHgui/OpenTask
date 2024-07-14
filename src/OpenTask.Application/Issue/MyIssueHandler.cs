using OpenTask.Application.Base.Commands;
using OpenTask.Application.Base;
using OpenTask.Domain.Workitems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Issue
{
    public class MyIssueHandler : ICommandHandler<MyIssueCommand, MyIssueResponse>
    {
        IWorkitemRepository issueRepository;

        public MyIssueHandler(IWorkitemRepository issueRepository)
        {
            this.issueRepository = issueRepository;
        }

        public Task<MyIssueResponse> Handle(MyIssueCommand request, CancellationToken cancellationToken)
        {
            var res = issueRepository.MyIssue(request.OrgId, request.UserId);
            return Task.FromResult(new MyIssueResponse
            {
                Issues = res
            });
        }
    }
}
