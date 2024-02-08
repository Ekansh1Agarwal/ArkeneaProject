using ArkeneaProject.Data;

namespace ArkeneaProject.Services
{
    public class FileServices : IFileServices
    {
        IWebHostEnvironment environment;
        private readonly ApplicationDbContext _dbContext;

        public FileServices(IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            environment = env;
        }


        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "ProfileUploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
                ;
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "ProfileUploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public UserData SaveData(UserData data)
        {
            _dbContext.UsersData.Add(data);
            _dbContext.SaveChanges();
            return data;
        }

        public async Task<List<UserData>> GetAllData()
        {
            var result = _dbContext.UsersData.ToList();
            return result;
        }
        public async Task<List<UserData>> GetAllUserData(ApplicationUser User)
        {
            if(User != null)
            {
                return _dbContext.UsersData.Where(x => x.Email.ToLower() == User.Email.ToLower())?.ToList();
            }
            return _dbContext.UsersData.ToList();

        }

        public async Task<UserData> UpdateDataAsync(UserData data)
        {
            _dbContext.Update(data);
            _dbContext.SaveChanges();
            return data;

        }

        public async Task<UserData> DeleteUserData(int id)
        {
            var userData = await _dbContext.UsersData.FindAsync(id);
            if (userData != null)
            {
                _dbContext.UsersData.Remove(userData);
            }
            await _dbContext.SaveChangesAsync();
            return null;
        }
    }
}
