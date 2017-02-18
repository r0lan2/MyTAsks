using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.Repositories;
using MyTasks.Domain;
using MyTasks.Data.Validators;
using MyTasks.Localization.Desktop;

namespace MyTasks.Data.UnitOfWorks
{
    public class LookupUnitOfWork
    {
        public readonly IWorkinghoursDataContext Context;
        public GenericRepository<IWorkinghoursDataContext, Customer> CustomerRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, Project> ProjectRepository { get; set; }
        private CustomerUnitOfWorkValidator _validator;

        public LookupUnitOfWork(IWorkinghoursDataContext dataContext)
        {
            Context = dataContext;
            CustomerRepository = new GenericRepository<IWorkinghoursDataContext, Customer>(Context);
            ProjectRepository = new GenericRepository<IWorkinghoursDataContext, Project>(Context);
        }


        public LookupUnitOfWork()
        {
            Context = new WorkinghoursDataContext();
            CustomerRepository = new GenericRepository<IWorkinghoursDataContext, Customer>(Context);
            ProjectRepository = new GenericRepository<IWorkinghoursDataContext, Project>(Context);
        }


        public List<Customer> GetCustomers()
        {
            var customers= CustomerRepository.All().ToList();
            foreach (var customer in customers)
            {
                customer.IsUsed = ProjectRepository.Where(p => p.CustomerId == customer.CustomerId).FirstOrDefault() !=null;
                customer.UsedCaption = customer.IsUsed ? Desktop.Yes : Desktop.No;
            }
            return customers;
        }

        public Customer GetCustomer(int customerId)
        {
            var customer =CustomerRepository.FindById(customerId);
            customer.IsUsed = ProjectRepository.Where(p => p.CustomerId == customer.CustomerId).FirstOrDefault()!=null;
            return customer;
        }


        public void AddCustomer(Customer customer)
        {
            CustomerRepository.Add(customer);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public bool CanDeleteCustomer(Customer customer)
        {
            var projectsBySelectedCustomer = ProjectRepository.Where(c => c.CustomerId == customer.CustomerId);
            return !projectsBySelectedCustomer.Any();
        }

        public ValidationStatus Validate(Customer customer)
        {
            _validator = new CustomerUnitOfWorkValidator(this);
            ValidationResult result = _validator.Validate(customer);
            return new ValidationStatus { IsValid = result.IsValid, ErrorMessage = _validator.ValidationMessage };
        }


    }
}
