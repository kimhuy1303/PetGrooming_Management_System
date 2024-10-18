using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IUserRepository
    {
        bool VerifyPassword(User user, string password);
        Task<ICollection<User>> GetAll();
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(int id);
        Task<bool> CreateUser(UserRequest userDTO);
        void DeleteUser(int id);
        Task<List<User>> SearchUser(string key);
        void ModifyUser(int id, ProfileRequest profileDTO);
    }
}
