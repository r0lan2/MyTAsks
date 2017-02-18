using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Glimpse.AspNet.Tab;
using MyTasks.Data.Repositories;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain.DataContracts;
using MyTasks.Domain.Entities;
using System.Collections.Generic;

//http://techbrij.com/crud-file-upload-asp-net-mvc-ef-multiple
namespace MyTasks.Web.Models
{


    public class FileViewModel
    {

        public List<FileData> SavingTicketFilesIfExists(int ticketId)
        {
            var request = HttpContext.Current.Request;
            if (request.Files.Count <= 0) return new List<FileData>();
            var partialTicketPath= MvcApplication.ApplicationSettings.TicketPath + ticketId;
            var ticketPath = HttpContext.Current.Server.MapPath("~/" + partialTicketPath);
            bool isExists = System.IO.Directory.Exists(ticketPath);
            if (!isExists)
                System.IO.Directory.CreateDirectory(ticketPath);

            List<FileData> fileDetails = new List<FileData>();
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    FileData fileDetail = new FileData()
                    {
                        FileName = fileName,
                        Extension = Path.GetExtension(fileName),
                        FileGuid = System.Guid.NewGuid().ToString(),
                        Path = ticketPath,
                        TicketDetailId = ticketId
                    };
                    fileDetails.Add(fileDetail);

                    var path = Path.Combine(ticketPath, fileDetail.FileGuid + fileDetail.Extension);
                    file.SaveAs(path);
                }
            }
            return fileDetails;
        }

       
        public FileData SaveUserProfilePicture(string userMail)
        {
            var request = HttpContext.Current.Request;
            if (request.Files.Count <= 0) return new FileData();

            var profilesPath = HttpContext.Current.Server.MapPath("~/"+ MvcApplication.ApplicationSettings.ProfilesPicturePath + userMail);
            bool isExists = System.IO.Directory.Exists(profilesPath);
            if (!isExists)
                System.IO.Directory.CreateDirectory(profilesPath);

          
            var file = request.Files[0];
            FileData profilePicture= new FileData();
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                profilePicture = new FileData()
                {
                    FileName = fileName,
                    Extension = Path.GetExtension(fileName),
                    FileGuid = System.Guid.NewGuid().ToString(),
                    Path = profilesPath

                };
                var path = Path.Combine(profilesPath, profilePicture.FileGuid + profilePicture.Extension);
                file.SaveAs(path);
            }
            
            return profilePicture;
        }


    }
}