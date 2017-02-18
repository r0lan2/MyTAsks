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
using MyTasks.Domain.DataContracts;

namespace MyTasks.Data.Validators
{
    public class TicketUnitOfWorkValidator : AbstractValidator<TicketData>
    {

        private StringBuilder _validationMessage = new StringBuilder();
        private TicketUnitOfWork unitOfWork;

        public TicketUnitOfWorkValidator(MyTasks.Data.UnitOfWorks.TicketUnitOfWork uo)
        {
            unitOfWork = uo;
            RuleFor(x => x.Title).NotEmpty().WithMessage(Localization.Desktop.Desktop.TitleIsRequired);
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage(Localization.Desktop.Desktop.ProjectIsRequired);
            RuleFor(x => x.AreaId).NotEmpty().WithMessage(Localization.Desktop.Desktop.AreaIdIsRequired);
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage(Localization.Desktop.Desktop.CategoryIdIsRequired);
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Localization.Desktop.Desktop.UserIdIsRequired);
            RuleFor(x => x.PriorityId).NotEmpty().WithMessage(Localization.Desktop.Desktop.PriorityIdIsRequired);
            RuleFor(x => x.Content).NotEmpty().WithMessage(Localization.Desktop.Desktop.ContentIsRequired);
        }

        public string ValidationMessage
        {
            get { return _validationMessage.ToString(); }
        }

        public override ValidationResult Validate(TicketData instance)
        {
            var result = base.Validate(instance);
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
