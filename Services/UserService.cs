using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.UserRepositories;

namespace TodoApi.UserServices
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);        Task<User> Create(User user);
        Task<User> Update(User user);
        Task<User> ChangePW(User userParam, string NewPassword);
        Task<User> Delete(int id);
    }
    

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        public IConfiguration Configuration { get; }
        private static byte[] _passwordSalt;
        public UserService(IUserRepository userRepository, 
            IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _passwordSalt = Encoding.UTF8.GetBytes(_appSettings.Secret);
        }

        public async Task<User> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            string passwordHash;
            CreatePasswordHash(password, out passwordHash);
            return await _userRepository.Authenticate(email, passwordHash);            
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _userRepository.GetUserEmail(email);
        }

        

        public async Task<User> Create(User user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email is required");
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password is required");
            var existingUser = await _userRepository.GetUserEmail(user.Email);
            if (existingUser != null)
                throw new Exception("Email \"" + user.Email + "\" is already taken");

            string passwordHash;
            CreatePasswordHash(user.Password, out passwordHash);

            var newUser = new User { 
                Email = user.Email,
                Password = passwordHash,
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            newUser = await _userRepository.Create(newUser);

            return newUser;            
        }

        public async Task<User> ChangePW(User userParam, string NewPassword)
        {
            var user = await _userRepository.GetUserById(userParam.Id);

            if (user == null)
                return null;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                string passwordHash;
                CreatePasswordHash(NewPassword, out passwordHash);
                user.Password = passwordHash;
            }else
                return null;            try
            {
                await _userRepository.ChangePW(user);
                return user;
            }
            catch (DbUpdateConcurrencyException e)
            {
                return null;

            }
        }

        public async Task<User> Update(User userParam)
        {
            var user = await _userRepository.GetUserById(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            user = await _userRepository.GetUserEmail(userParam.Email);
            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Id != user.Id )
            {
                // new email is already taken                
                    return null;
            }
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Email))
                user.Email = userParam.Email;
            
            // update password if provided
            if (!string.IsNullOrWhiteSpace(userParam.Password))
            {
                string passwordHash;
                CreatePasswordHash(userParam.Password, out passwordHash);
                user.Password = passwordHash;
            }
            user.Updated = DateTime.Now;            
            return await _userRepository.Update(user);            
        }

        public async Task<User> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }
        // private helper methods
        private static void CreatePasswordHash(string password, out string passwordHash)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (password.Length < 3) throw new ArgumentException("Value cannot be less than 3 characters.", "new password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            using (var hmac = new System.Security.Cryptography.HMACSHA512(_passwordSalt))
            {
                var pwHash = hmac.ComputeHash(System.Text.Encoding.Default.GetBytes(password));
                passwordHash = System.Text.Encoding.Default.GetString(pwHash);
            }
        }
        // private helper methods
        private static bool VerifyPasswordHash(string password, string storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            var computedHash = System.Text.Encoding.Default.GetBytes(password);
            if (password != storedHash) return false;
            return true;
        }

    }
}
