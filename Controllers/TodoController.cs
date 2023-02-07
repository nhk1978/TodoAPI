using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.TodoServices;
using TodoApi.UserServices;

namespace TodoApi.Controllers
{
    //[Route("api/v1/[controller]")]
    [Route("api/v1/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        //private readonly DataContext _context;
        private readonly ITodoService _todoService;
        private readonly IUserService _userService;
        private Helper _helper = new Helper();
        private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();
        private readonly AppSettings _appSettings; 
        public TodoController(ITodoService todoService, IUserService userService,
            IOptions<AppSettings> appSettings)
        {
            _todoService = todoService;
            _userService = userService;
            _helper = new Helper();
            _appSettings = appSettings.Value;
        }

        // GET: api/v1/todos?status=[status]
        //[HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetTodo(todo_status status)
        {
            if (!_helper.CheckBearerToken(Request, _jwtHandler, _appSettings))
            {
                return Unauthorized();
            }
            var todos =  _todoService.GetTodo(status);
            return Ok(todos);
        }       

        // PUT: api/v1/todos/:id
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, TodoDTO todoDTO)
        {
            var user = await _userService.GetById(todoDTO.UserID);
            if (user == null) return Ok(new { message = ("This user is not exsiting") });
            var todo = await _todoService.GetTodoById(id);
            if (todo == null) {
                return NotFound();
            }

            todo.Name = todoDTO.Name;
            todo.Description = todoDTO.Description;
            todo.UserID = todoDTO.UserID;
            todo.Updated = DateTime.Now;
            todo.Status = todoDTO.Status;
            
            todo =  await _todoService.Update(todo);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(new { message = "Updated suscessful" });
        }
       

        // POST: api/v1/todos/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(TodoDTO todoDTO)
        {
            var user = await _userService.GetById(todoDTO.UserID);
            if (user == null) return Ok(new { message = ("This user is not exsiting") });
            var todo = new Todo
            {
                Name = todoDTO.Name,
                Description = todoDTO.Description,
                UserID = todoDTO.UserID,
                Updated = DateTime.Now,
                Created = DateTime.Now,
                Status = todoDTO.Status
            };

            var newTodo = await _todoService.Create(todo);
            //await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodo),
                new { id = newTodo.Id },
                new
                {
                    Name = newTodo.Name,
                    Description = newTodo.Description,
                    UserID = newTodo.UserID,
                    Status = newTodo.Status == todo_status.Completed ? "Completed" : newTodo.Status == todo_status.OnGoing ? "OnGoing" : "NotStarted"
                }
            );
        }

        // DELETE: api/v1/Todos/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _todoService.Delete(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(new { message = "Deleted successfully" });        }

        // OPTIONS: api/v1/Todos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpOptions]
        public async Task<IActionResult> OptionsTodo()
        {

            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");

            return Ok();
        }*/

    }
}
