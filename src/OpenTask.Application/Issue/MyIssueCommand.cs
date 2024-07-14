using OpenTask.Application.Base.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Issue
{
    public class MyIssueCommand : ICommand<MyIssueResponse>
    {
        public long UserId { get; set; }

        public long OrgId { get; set; }
    }
}
