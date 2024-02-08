using ArkeneaProject.Data;

namespace ArkeneaProject.Services
{
    public interface IFileServices
    {
        Task<List<UserData>> GetAllData();
        Task<List<UserData>> GetAllUserData(ApplicationUser User = null);
        UserData SaveData(UserData data);
        Task<UserData> UpdateDataAsync(UserData data);
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
        Task<UserData> DeleteUserData(int id);
    }
}
