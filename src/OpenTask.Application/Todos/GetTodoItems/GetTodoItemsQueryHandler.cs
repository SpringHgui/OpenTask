using OpenTask.Application.Base.Queries;
using OpenTask.Domain.TodoItems;

namespace OpenTask.Application.Todos.GetTodoItems
{
    public class GetTodoItemsQueryHandler : IQueryHandler<GetTodoItemsQuery, TodoItemResponse>
    {
        readonly ITodoItemRepository todoItemRepository;

        public GetTodoItemsQueryHandler(ITodoItemRepository todoItemRepository)
        {
            this.todoItemRepository = todoItemRepository;
        }

        public Task<TodoItemResponse> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            var res = todoItemRepository.Find(request.Id);

            return Task.FromResult(new TodoItemResponse
            {
                TodoInfo = new TodoDto
                {
                    Id = request.Id,
                    Summary = res.Summary,
                    Title = res.Title,
                },
            });
        }
    }
}
