using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.Repositories;
using MyTasks.Domain;
using MyTasks.Domain.Entities;

namespace MyTasks.Data.UnitOfWorks
{
    public class FileDataUnitOfWork
    {
        private readonly IWorkinghoursDataContext context;
        public GenericRepository<IWorkinghoursDataContext, FileData> FileDataRepository { get; set; }

        public FileDataUnitOfWork(IWorkinghoursDataContext dataContext)
        {
            context = dataContext;
            InitRepositories();
        }


        public FileDataUnitOfWork()
        {
            context = new WorkinghoursDataContext();
            InitRepositories();
        }

        public void InitRepositories()
        {
            FileDataRepository = new GenericRepository<IWorkinghoursDataContext, FileData>(context);
        }

        public FileData GetPictureByUser(int pictureId )
        {
            var fileData=  FileDataRepository.Where(s => s.FileId == pictureId);
            return fileData.FirstOrDefault();
        }

        public int AddPhotoProfile(FileData picture)
        {
            FileDataRepository.Add(picture);
            Save();
            return picture.FileId;
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
