using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.Utils;
using PetGrooming_Management_System.Configs.Constant;


namespace PetGrooming_Management_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDBContext _dbContext;
        public UserRepository(MainDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateUser(UserRequest userDTO)
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
            var res = await _dbContext.SaveChangesAsync();
            return res > 0;
        }

        public async Task DeleteUser(int id)
        {
            var _user = await GetUserById(id);
            _dbContext.Users.Remove(_user!);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == Config.Constant.Role.Customer);
            return _user;
        }

        public async Task<List<User>> SearchUser(string key)
        {
            var result = await _dbContext.Users.Where(e => e.Role == Config.Constant.Role.Customer && e.Username!.Contains(key) || e.FullName!.Contains(key) || e.Email!.Contains(key) || e.PhoneNumber!.Contains(key)).ToListAsync();
            return result;
        }

        public async Task<ICollection<User>> GetAll()
        {
            var result = await _dbContext.Users.Where(e => e.Role == Config.Constant.Role.Customer).ToListAsync();
            return result;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var _user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Role == Config.Constant.Role.Customer && e.Username!.Equals(username));
            return _user;
        }

        public bool VerifyPassword(User user,  string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public async Task<User> ModifyUser(int id, ProfileRequest request)
        {
            // UploadFile cho avatar 
            var avatarPath = UploadFile.GetFilePath(request.AvatarPath!);
           
            var _user = await GetUserById(id);
            _user.FullName = request.FullName;
            _user.Email = request.Email;
            _user.Address = request.Address;
            _user.Gender = request.Gender;
            _user.PhoneNumber = request.PhoneNumber;
            _user.AvatarPath = avatarPath;
            _user.DateOfBirth = request.DateOfBirth;
            _dbContext.SaveChanges();
            return _user;
        }
    }
}
