using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.Repositories;
using MyTasks.Domain;

namespace MyTasks.Data.UnitOfWorks
{
    public class SetupUnitOfWork
    {
        private readonly IWorkinghoursDataContext context;
        public GenericRepository<IWorkinghoursDataContext, MyTasks.Domain.Dbversion> DbVersionRepository { get; set; }
        public ApplicationSettingsRepository ApplicationSettingsRepository { get; set; }

        public SetupUnitOfWork(IWorkinghoursDataContext dataContext)
        {
            context = dataContext;
            DbVersionRepository = new GenericRepository<IWorkinghoursDataContext, Dbversion>(context);
            ApplicationSettingsRepository = new ApplicationSettingsRepository(context);
        }


        public SetupUnitOfWork()
        {
            context = new WorkinghoursDataContext();
            DbVersionRepository = new GenericRepository<IWorkinghoursDataContext, Dbversion>(context);
            ApplicationSettingsRepository = new ApplicationSettingsRepository(context);
        }


        public List<ApplicationSettings> GetSettings()
        {
            return ApplicationSettingsRepository.All().ToList();
        }


        public void SaveChanges()
        {
            context.SaveChanges();
        }


    }
}
