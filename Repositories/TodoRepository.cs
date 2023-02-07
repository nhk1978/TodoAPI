using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Helpers;
using TodoApi.Models;

namespace TodoApi.TodoRepositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAll();
        Task<Todo> GetTodoById(int id);
        List<Todo> GetTodo(todo_status status);
        Task<Todo> Create(Todo Todo);
        Task<Todo> Update(Todo Todo);
        Task<Todo> Delete(int id);
    }
    

    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;
        public IConfiguration Configuration { get; }
        public TodoRepository(DataContext context)
        {
            _context = context;
        }


        public async  Task<IEnumerable<Todo>> GetAll()
        {
            return  await _context.Todo.ToListAsync();
        }

        public async  Task<Todo> GetTodoById(int id)
        {
            return await _context.Todo.FindAsync(id);
        }

        public  List<Todo> GetTodo(todo_status status)
        {
            return  _context.Todo.Where(x => x.Status == status).ToList();           
        }

        public async  Task<Todo> Create(Todo todo)
        {
            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

       

        public async  Task<Todo> Update(Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;

        }

        public async Task<Todo> Delete(int id)
        {
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null) return null;            
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return todo;

        }

    }
}
