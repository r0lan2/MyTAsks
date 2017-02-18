using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MyTasks.Domain.Entities.Validators
{
    public class ProjectValidator : AbstractValidator<Domain.Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.ProjectName).NotEmpty().WithMessage(Localization.Desktop.Desktop.ProjectNameRequired);
            RuleFor(x => x.CustomerId).NotNull().WithMessage(Localization.Desktop.Desktop.CustomerIsRequired);
        }

    }
}
