using BookStoreAPI.Services.CustomerSevice.DTOs;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.CustomerSevice.Interfaces;
using BookStoreAPI.Services.CustomerSevice.Repositories;
using Microsoft.AspNetCore.Localization;

namespace BookStoreAPI.Services.CustomerSevice
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService (ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(c => new CustomerDTO
            {
                Id = c.Id,
                FamilyName = c.FamilyName,
                GivenName = c.GivenName,
                DateOfBirth = c.DateOfBirth,
                Address = c.Address,
                Phone = c.Phone,
                Gender = c.Gender,
                IsDeleted = c.IsDeleted
            });
        }

        public async Task<CustomerDTO?> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id '{id}' not found.");

            return new CustomerDTO
            {
                Id = customer.Id,
                FamilyName = customer.FamilyName,
                GivenName = customer.GivenName,
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                Phone = customer.Phone,
                Gender = customer.Gender,
                IsDeleted = customer.IsDeleted
            };
        }

        public async Task<CustomerDTO?> GetByPhoneAsync(string phone)
        {
            var customer = await _customerRepository.GetByPhoneAsync(phone);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with phone '{phone}' not found.");

            return new CustomerDTO
            {
                Id = customer.Id,
                FamilyName = customer.FamilyName,
                GivenName = customer.GivenName,
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                Phone = customer.Phone,
                Gender = customer.Gender,
                IsDeleted = customer.IsDeleted
            };
        }

        public async Task<IEnumerable<CustomerDTO>> SearchByKeywordAsync(string keyword)
        {
            var customers = await _customerRepository.SearchByKeywordAsync(keyword);

            return customers.Select(c => new CustomerDTO
            {
                Id = c.Id,
                FamilyName = c.FamilyName,
                GivenName = c.GivenName,
                DateOfBirth = c.DateOfBirth,
                Address = c.Address,
                Phone = c.Phone,
                Gender = c.Gender,
                IsDeleted = c.IsDeleted
            });
        }

        public async Task<bool> AddAsync(CustomerCreateDTO customerCreateDTO)
        {
            if (!IsOver18(customerCreateDTO.DateOfBirth))
                throw new ArgumentException("Customer have to equal or over 18.");

            var existingCustomer = await _customerRepository.GetByPhoneAsync(customerCreateDTO.Phone);
            if (existingCustomer != null)
                throw new InvalidOperationException("A customer with the same phone already exists.");

            var customer = new Customer
            {
                FamilyName = customerCreateDTO.FamilyName,
                GivenName = customerCreateDTO.GivenName,
                DateOfBirth = customerCreateDTO.DateOfBirth,
                Address = customerCreateDTO.Address,
                Phone = customerCreateDTO.Phone,
                Gender = customerCreateDTO.Gender
            };

            await _customerRepository.AddAsync(customer);

            return await _customerRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, CustomerUpdateDTO customerUpdateDTO)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with id '{id}' not found.");

            if (!IsOver18(customerUpdateDTO.DateOfBirth))
                throw new ArgumentException("Customer have to equal or over 18.");
            
            var duplicateCustomer = await _customerRepository.GetByPhoneAsync(customerUpdateDTO.Phone);
            if (duplicateCustomer != null && duplicateCustomer.Id != id)
                throw new InvalidOperationException("A customer with the same phone already exists.");

            existingCustomer.FamilyName = customerUpdateDTO.FamilyName;
            existingCustomer.GivenName = customerUpdateDTO.GivenName;
            existingCustomer.DateOfBirth = customerUpdateDTO.DateOfBirth;
            existingCustomer.Address = customerUpdateDTO.Address;
            existingCustomer.Phone = customerUpdateDTO.Phone;
            existingCustomer.Gender = customerUpdateDTO.Gender;
            existingCustomer.IsDeleted = customerUpdateDTO.IsDeleted;

            _customerRepository.Update(existingCustomer);

            return await _customerRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);

            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with id '{id}' not found.");

            if (existingCustomer.IsDeleted == true)
                throw new InvalidOperationException($"Customer with id {id} is already deleted.");

            _customerRepository.Delete(existingCustomer);

            return await _customerRepository.SaveChangesAsync();
        }

        public bool IsOver18(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
                age--;

            return age >= 18;
        }

    }
}
