using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Helpers;
using TodoApi.Models;

namespace TodoApi.UserRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUserById(int id);
        Task<User> GetUserEmail(string email);
        Task<User> Authenticate(string email,string password);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task<User> ChangePW(User user);
        Task<User> Delete(int id);
    }


    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public IConfiguration Configuration { get; }
        public UserRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserEmail(string email)
        {
            // validation
            var newUser = _context.Users.SingleOrDefault(x => x.Email == email);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            // validation
            var user = _context.Users.SingleOrDefault(x => x.Email == email && x.Password == password);
            await _context.SaveChangesAsync();
            return user;
        }
        

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<User> ChangePW(User user)
        {          
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
