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


namespace MyTasks.Data.Validators
{
    public class CustomerUnitOfWorkValidator : AbstractValidator<Customer>
    {
        private StringBuilder _validationMessage = new StringBuilder();
        private LookupUnitOfWork unitOfWork;

        public CustomerUnitOfWorkValidator(LookupUnitOfWork uo)
        {
            unitOfWork = uo;
            RuleFor(x => x.Name).NotEmpty().WithMessage(Localization.Desktop.Desktop.CustomerIsRequired);
            RuleFor(customer => customer.Name).Must(UniqueName).WithMessage("This Customer name already exists.");
        }

        public string ValidationMessage
        {
            get { return _validationMessage.ToString(); }
        }

        private bool UniqueName(Customer instance, string name)
        {

            var customer = unitOfWork.Context.Customer.SingleOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (customer == null) return true;
            return customer.CustomerId == instance.CustomerId;
        }

        public override ValidationResult Validate(Customer instance)
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
