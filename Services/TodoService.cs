using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.TodoRepositories;

namespace TodoApi.TodoServices
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAll();
        Task<Todo> GetTodoById(int id);
        List<Todo> GetTodo(todo_status status);
        Task<Todo> Create(Todo todo);
        Task<Todo> Update(Todo todo);
        Task<Todo> Delete(int id);
    }
    

    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;        public IConfiguration Configuration { get; }
        //private static byte[] _passwordSalt;
        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public async Task<IEnumerable<Todo>> GetAll()
        {
            return await _todoRepository.GetAll();        }

        public async Task<Todo> GetTodoById(int id)
        {
            return await _todoRepository.GetTodoById(id);
        }

        public  List<Todo> GetTodo(todo_status status)
        {            
            return  _todoRepository.GetTodo(status);
        }

        public async Task<Todo> Create(Todo todo)
        {
            return await _todoRepository.Create(todo);        }

        public async Task<Todo> Update(Todo todo)
        {
            return await _todoRepository.Update(todo);
        }

        public async Task<Todo> Delete(int id)
        {
               return await _todoRepository.Delete(id);
        }

    }
}
