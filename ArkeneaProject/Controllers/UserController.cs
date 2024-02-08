using ArkeneaProject.Data;
using ArkeneaProject.Models;
using ArkeneaProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace ArkeneaProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IFileServices _fileServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(IFileServices fileServices,
                              UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _fileServices = fileServices;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAll()  //Getting all data for a User
        {
            var User = getUserEmail();
            if (User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userDatas = await _fileServices.GetAllUserData(User);
            return View(userDatas);
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult Upload() //View for Upload data
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(UserData userData, IFormFile file) //Uploading a User data
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    //data.
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    userData.fileName = file.FileName;
                    userData.fileType = file.ContentType;
                    userData.file = fileBytes;
                }
                var output = _fileServices.SaveData(userData);
                if (output.Id > 0)
                {
                    ViewData["Message"] = "Record Save Successfull";
                    return RedirectToAction("index", "home");
                }
            }
            ViewData["Message"] = "Record Save Unsuccessfull";
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Edit(int? id) //View Page for Edit Record
        {
            if (id == null || _fileServices.GetAllUserData() == null)
            {
                return NotFound();
            }

            var userData = await _fileServices.GetAllUserData();
            if (userData == null || !userData.Exists(x => x.Id == id))
            {
                return NotFound();
            }
            return View(userData.Where(x => x.Id == id).FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserData userData, IFormFile file) //For Editing any record
        {
            if (id != userData.Id)
            {
                return NotFound();
            }
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    //data.
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    userData.fileName = file.FileName;
                    userData.fileType = file.ContentType;
                    userData.file = fileBytes;
                }
            }
            _fileServices.UpdateDataAsync(userData);

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Delete(int id) //For Deletion of Record
        {
            var User = getUserEmail();
            if(User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userDatas = await _fileServices.GetAllUserData(User);
            if (userDatas == null)
            {
                return Problem("No record present for the particular ID");
            }
            await _fileServices.DeleteUserData(id);
            return RedirectToAction("index", "home");
        }

        //[HttpPost]
        public IActionResult Download(int id) //For downloading the file
        {
            var response = new HttpResponseMessage();
            var User = getUserEmail();
            if (User == null)
            {
                return null;
            }
            var dataSet = _fileServices.GetAllUserData(User).Result;
            var data = dataSet.Where(x => x.Id == id).FirstOrDefault();
            return File(data.file, data.fileType, data.fileName);
            //return DownloadFile(data.file, data.fileName);
        }
        public ApplicationUser getUserEmail() //Getting the User type: Admin or User
        {
            var userID = _userManager.GetUserId(HttpContext.User);
            if (userID == null)
            {
                return null;
            }
            var User = _userManager.FindByIdAsync(userID).Result;
            return User;
        }
    }
}

