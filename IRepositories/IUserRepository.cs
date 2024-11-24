using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IUserRepository
    {
        bool VerifyPassword(User user, string password);
        Task<ICollection<User>> GetAll(int page, int size);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(int id);
        Task<bool> CreateUser(UserRequest userDTO);
        Task DeleteUser(int id);
        Task<List<User>> SearchUser(string key);
        Task<User> ModifyUser(int id, ProfileRequest profileDTO);
        Task<ProfileResponse> ViewProfile(int id);
    }
}
