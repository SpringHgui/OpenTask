using OpenTask.Domain.Workitems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Issue
{
    public class MyIssueResponse
    {
        public IEnumerable<Workitem> Issues { get; internal set; }
    }
}
