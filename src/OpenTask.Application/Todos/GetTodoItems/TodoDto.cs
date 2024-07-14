using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Todos.GetTodoItems
{
    public class TodoDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }
    }
}
