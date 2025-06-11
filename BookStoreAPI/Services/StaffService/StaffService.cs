using BookStoreAPI.Services.StaffService.DTOs;
using BookStoreAPI.Services.StaffService.Interfaces;
using BookStoreAPI.Services.StaffService.Repositories;

namespace BookStoreAPI.Services.StaffService
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<StaffDTO>> GetAllAsync()
        {
            var staffs = await _staffRepository.GetAllAsync();

            return staffs.Select(s => new StaffDTO
            {
                Id = s.Id,
                FamilyName = s.FamilyName,
                GivenName = s.GivenName,
                DateOfBirth = s.DateOfBirth,
                Address = s.Address,
                Phone = s.Phone,
                Email = s.Email,
                CitizenIdentification = s.CitizenIdentification,
                Role = s.Role,
                Gender = s.Gender,
                IsActived = s.IsActived,
                IsDeleted = s.IsDeleted
            });
        }

        public async Task<StaffDTO?> GetByIdAsync(Guid id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            if (staff == null)
                throw new KeyNotFoundException($"Staff with id '{id}' not found.");

            return new StaffDTO
            {
                Id = staff.Id,
                FamilyName = staff.FamilyName,
                GivenName = staff.GivenName,
                DateOfBirth = staff.DateOfBirth,
                Address = staff.Address,
                Phone = staff.Phone,
                Email = staff.Email,
                CitizenIdentification = staff.CitizenIdentification,
                Role = staff.Role,
                Gender = staff.Gender,
                IsActived = staff.IsActived,
                IsDeleted = staff.IsDeleted
            };
        }

        public async Task<StaffDTO?> GetByPhoneAsync(string phone)
        {
            var staff = await _staffRepository.GetByPhoneAsync(phone);

            if (staff == null)
                throw new KeyNotFoundException($"Staff with phone '{phone}' not found.");

            return new StaffDTO
            {
                Id = staff.Id,
                FamilyName = staff.FamilyName,
                GivenName = staff.GivenName,
                DateOfBirth = staff.DateOfBirth,
                Address = staff.Address,
                Phone = staff.Phone,
                Email = staff.Email,
                CitizenIdentification = staff.CitizenIdentification,
                Role = staff.Role,
                Gender = staff.Gender,
                IsActived = staff.IsActived,
                IsDeleted = staff.IsDeleted
            };
        }

        public async Task<StaffDTO?> GetByEmailAsync(string email)
        {
            var staff = await _staffRepository.GetByEmailAsync(email);

            if (staff == null)
                throw new KeyNotFoundException($"Staff with email '{email}' not found.");

            return new StaffDTO
            {
                Id = staff.Id,
                FamilyName = staff.FamilyName,
                GivenName = staff.GivenName,
                DateOfBirth = staff.DateOfBirth,
                Address = staff.Address,
                Phone = staff.Phone,
                Email = staff.Email,
                CitizenIdentification = staff.CitizenIdentification,
                Role = staff.Role,
                Gender = staff.Gender,
                IsActived = staff.IsActived,
                IsDeleted = staff.IsDeleted
            };
        }

        public async Task<StaffDTO?> GetByCitizenIdentificationAsync(string citizenIdentification)
        {
            var staff = await _staffRepository.GetByCitizenIdentificationAsync(citizenIdentification);

            if (staff == null)
                throw new KeyNotFoundException($"Staff with citizen identification '{citizenIdentification}' not found.");

            return new StaffDTO
            {
                Id = staff.Id,
                FamilyName = staff.FamilyName,
                GivenName = staff.GivenName,
                DateOfBirth = staff.DateOfBirth,
                Address = staff.Address,
                Phone = staff.Phone,
                Email = staff.Email,
                CitizenIdentification = staff.CitizenIdentification,
                Role = staff.Role,
                Gender = staff.Gender,
                IsActived = staff.IsActived,
                IsDeleted = staff.IsDeleted
            };
        }

        public async Task<IEnumerable<StaffDTO>> SearchByKeywordAsync(string keyword)
        {
            var staffs = await _staffRepository.SearchByKeyword(keyword);

            return staffs.Select(s => new StaffDTO
            {
                Id = s.Id,
                FamilyName = s.FamilyName,
                GivenName = s.GivenName,
                DateOfBirth = s.DateOfBirth,
                Address = s.Address,
                Phone = s.Phone,
                Email = s.Email,
                CitizenIdentification = s.CitizenIdentification,
                Role = s.Role,
                Gender = s.Gender,
                IsActived = s.IsActived,
                IsDeleted = s.IsDeleted
            });
        }

        public async Task<bool> UpdateAsync(Guid id, StaffUpdateDTO staffUpdateDTO)
        {
            if (!IsOver18(staffUpdateDTO.DateOfBirth))
                throw new ArgumentException("Staff must be at least 18 years old.");

            if (!IsValidEmail(staffUpdateDTO.Email))
                throw new ArgumentException("Staff email is not in a valid format.");

            var existingStaff = await _staffRepository.GetByIdAsync(id);

            if (existingStaff == null)
                throw new KeyNotFoundException($"Staff with id '{id}' not found.");

            var duplicatePhone = await _staffRepository.GetByPhoneAsync(staffUpdateDTO.Phone);
            if (duplicatePhone != null && id != duplicatePhone.Id)
                throw new InvalidOperationException("A staff with the same phone already exists.");

            var duplicateEmail = await _staffRepository.GetByEmailAsync(staffUpdateDTO.Email);
            if (duplicateEmail != null && id != duplicateEmail.Id)
                throw new InvalidOperationException("A staff with the same email already exists.");

            var duplicateCitizenIdentification = await _staffRepository.GetByCitizenIdentificationAsync(staffUpdateDTO.CitizenIdentification);
            if (duplicateCitizenIdentification != null && id != duplicateCitizenIdentification.Id)
                throw new InvalidOperationException("A staff with the same citizen identification already exists.");

            existingStaff.FamilyName = staffUpdateDTO.FamilyName;
            existingStaff.GivenName = staffUpdateDTO.GivenName;
            existingStaff.DateOfBirth = staffUpdateDTO.DateOfBirth;
            existingStaff.Address = staffUpdateDTO.Address;
            existingStaff.Phone = staffUpdateDTO.Phone;
            existingStaff.Email = staffUpdateDTO.Email;
            existingStaff.CitizenIdentification = staffUpdateDTO.CitizenIdentification;
            existingStaff.Role = staffUpdateDTO.Role;
            existingStaff.Gender = staffUpdateDTO.Gender;
            existingStaff.IsActived = staffUpdateDTO.IsActived;
            existingStaff.IsDeleted = staffUpdateDTO.IsDeleted;

            _staffRepository.Update(existingStaff);

            return await _staffRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingStaff = await _staffRepository.GetByIdAsync(id);

            if (existingStaff == null)
                throw new KeyNotFoundException($"Staff with id '{id}' not found");

            if (existingStaff.IsDeleted == true)
                throw new InvalidOperationException($"Staff with id {id} is already deleted.");

            _staffRepository.Delete(existingStaff);

            return await _staffRepository.SaveChangesAsync();
        }

        private bool IsOver18(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (dateOfBirth > today)
                throw new ArgumentException("Date of birth cannot be in the future.");

            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
                age--;

            return age >= 18;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
