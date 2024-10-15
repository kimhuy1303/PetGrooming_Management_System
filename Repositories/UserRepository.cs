using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;


namespace PetGrooming_Management_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDBContext _dbContext;
        public UserRepository(MainDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> createUser(UserRequest userDTO)
        {
            var _user = new User
            {
                Username = userDTO.UserName,
                // Hash password
                Password = userDTO.Password,
                PhoneNumber = userDTO.PhoneNumber,
                Role = userDTO.Role,
                CreatedDate = DateTime.UtcNow

            };
            await _dbContext.AddAsync(_user);
            var res = _dbContext.SaveChanges();
            return res > 0;
        }

        public async void deleteUser(int id)
        {
            var _user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            _dbContext.Users.Remove(_user!);
            _dbContext.SaveChanges();
        }

        public async Task<User> getUserById(int id)
        {
            var _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return _user;
        }

        public async Task<List<User>> searchUser(string key)
        {
            var result = await _dbContext.Users.Where(e => e.Username!.Contains(key)).ToListAsync();
            return result;
        }

        public async Task<ICollection<User>> getAll()
        {
            var result = await _dbContext.Users.ToListAsync();
            return result;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var _user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Username!.Equals(username));
            return _user!;
        }

        public bool verifyPassword(User user,  string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
