using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;

namespace MyTasks.Data
{
    public class ProjectUnitOfWorkValidator : AbstractValidator<Project>
    {
        private StringBuilder _validationMessage= new StringBuilder();
        private ProjectUnitOfWork unitOfWork;
        public ProjectUnitOfWorkValidator(MyTasks.Data.UnitOfWorks.ProjectUnitOfWork uo)
        {
            unitOfWork = uo;
            RuleFor(x => x.ProjectName).NotEmpty().WithMessage(Localization.Desktop.Desktop.ProjectNameRequired);
            RuleFor(project=> project.ProjectName).Must(UniqueName).WithMessage("This Project name already exists.");
        }

        public string ValidationMessage {
            get { return _validationMessage.ToString(); }
        }

        private bool UniqueName(Project instance,string name )
        {
            
            var project = unitOfWork.Context.Project.SingleOrDefault(x => x.ProjectName.ToLower() == name.ToLower());

            if (project == null) return true;
            return project.ProjectId == instance.ProjectId;
        }

        public override ValidationResult Validate(Project instance)
        { 
            var result= base.Validate(instance);
            if (!result.IsValid)
            {
                _validationMessage = new StringBuilder();
                foreach (var failure in result.Errors)
                {
                    _validationMessage.Append(failure.ErrorMessage).Append(" ");
                }
            }
            return result;
        }
    }
}
