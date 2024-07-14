using OpenTask.Application.Base.Queries;

namespace OpenTask.Application.Todos.GetTodoItems
{
    public class GetTodoItemsQuery : IQuery<TodoItemResponse>
    {
        public long Id { get; set; }
    }
}
