using ArkeneaProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArkeneaProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IFileServices _fileServices;

        public AdminController(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var userDatas = await _fileServices.GetAllData();
            return View(userDatas);
        }
        public IActionResult Download(int id)
        {
            var dataSet = _fileServices.GetAllData().Result;
            var data = dataSet.Where(x => x.Id == id).FirstOrDefault();
            return File(data.file, data.fileType, data.fileName);
        }
    }
}
