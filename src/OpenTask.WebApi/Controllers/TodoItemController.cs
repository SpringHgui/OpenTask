using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using OpenTask.Application.Base;
using OpenTask.Application.Todos.GetTodoItems;
using System.ComponentModel.DataAnnotations;

namespace OpenTask.WebApi.Controllers
{
    public class TodoItemController : BaseController
    {
        public TodoItemController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<TodoItemResponse> Get([FromServices] IMediator mediator, [Range(1, double.MaxValue)] long Id)
        {
            return await RequestAsync<GetTodoItemsQuery, TodoItemResponse>(new GetTodoItemsQuery { Id = Id });
        }

        [HttpPut]
        public IActionResult Put(TodoDto item)
        {
            return Content($"insert");
        }

        [HttpPost]
        public IActionResult Post(TodoDto item)
        {
            return Content($"updated");
        }

        [HttpDelete]
        public IActionResult Delete(long Id)
        {
            return Content($"deleted");
        }
    }
}
