using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IUserRepository
    {
        bool verifyPassword(User user, string password);
        Task<ICollection<User>> getAll();
        Task<User> GetUserByUsername(string username);
        Task<User> getUserById(int id);
        Task<bool> createUser(UserRequest userDTO);
        void deleteUser(int id);
        Task<List<User>> searchUser(string key);
    }
}
