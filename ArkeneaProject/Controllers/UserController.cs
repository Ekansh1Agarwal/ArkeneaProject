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
        public async Task<IActionResult> GetAll()
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
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(UserData userData, IFormFile file)
        {
            //UserData userData = JsonConvert.DeserializeObject<UserData>(data.User);
            //if(data.file.Length > 0)
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

        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, UserData userData, IFormFile file)
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

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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
        public IActionResult Download(int id)
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

        public static HttpResponseMessage DownloadFile(byte[] byteArray, string fileName)
        {
            //var response = new HttpResponseMessage();


            var response = new HttpResponseMessage();
            string fileMediaType = "application/octet-stream";
            response.Content = new ByteArrayContent(byteArray);
            response.Content.Headers.ContentDisposition
              = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType
               = new MediaTypeHeaderValue(fileMediaType);
            response.Content.Headers.Add("Access-Control-Allow-Headers", "fileName");
            response.Content.Headers.Add("fileName", fileName);

            response.StatusCode = HttpStatusCode.OK;

            response.Content.Headers.ContentLength = byteArray.Length;
            return response;

        }



        public ApplicationUser getUserEmail()
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

