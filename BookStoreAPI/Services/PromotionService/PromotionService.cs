using BookStoreAPI.Services.PromotionService.DTOs;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.PromotionService.Interfaces;
using BookStoreAPI.Services.PromotionService.Repositories;

namespace BookStoreAPI.Services.PromotionService
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<IEnumerable<PromotionDTO>> GetAllAsync()
        {
            var promotions = await _promotionRepository.GetAllAsync();

            return promotions.Select(p => new PromotionDTO
            {
                Id = p.Id,
                Name = p.Name,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Condition = p.Condition,
                DiscountPercent = p.DiscountPercent,
                Quantity = p.Quantity,
                IsDeleted = p.IsDeleted
            });
        }

        public async Task<PromotionDTO?> GetByIdAsync(Guid id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);

            if (promotion == null)
                throw new KeyNotFoundException($"Promotion with id '{id}' not found.");

            return new PromotionDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                Condition = promotion.Condition,
                DiscountPercent = promotion.DiscountPercent,
                Quantity = promotion.Quantity,
                IsDeleted = promotion.IsDeleted
            };
        }

        public async Task<PromotionDTO?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Promotion name cannot be null or empty.");

            var promotion = await _promotionRepository.GetByNameAsync(name);

            if (promotion == null)
                throw new KeyNotFoundException($"Promotion with name '{name}' not found.");

            return new PromotionDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                Condition = promotion.Condition,
                DiscountPercent = promotion.DiscountPercent,
                Quantity = promotion.Quantity,
                IsDeleted = promotion.IsDeleted
            };
        }

        public async Task<IEnumerable<PromotionDTO>> SearchByKeywordAsync(string keyword)
        {
            var promotions = await _promotionRepository.SearchByKeywordAsync(keyword);

            return promotions.Select(p => new PromotionDTO
            {
                Id = p.Id,
                Name = p.Name,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Condition = p.Condition,
                DiscountPercent = p.DiscountPercent,
                Quantity = p.Quantity,
                IsDeleted = p.IsDeleted
            });
        }

        public async Task<bool> AddAsync(PromotionCreateDTO promotionCreateDTO)
        {
            var now = DateTime.Now;

            if (promotionCreateDTO.StartDate < now)
                throw new ArgumentException("StartDate must be in the future.");

            if (promotionCreateDTO.EndDate <= now)
                throw new ArgumentException("EndDate must be in the future.");

            if (promotionCreateDTO.StartDate >= promotionCreateDTO.EndDate)
                throw new ArgumentException("StartDate must be earlier than EndDate.");

            if (promotionCreateDTO.Condition < 0)
                throw new ArgumentException("Condition must be non-negative.");

            if (promotionCreateDTO.DiscountPercent <= 0 || promotionCreateDTO.DiscountPercent > 100)
                throw new ArgumentException("DiscountPercent must be between 1 and 100.");

            if (promotionCreateDTO.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");

            var promotion = new Promotion
            {
                Name = promotionCreateDTO.Name,
                StartDate = promotionCreateDTO.StartDate,
                EndDate = promotionCreateDTO.EndDate,
                Condition = promotionCreateDTO.Condition,
                DiscountPercent = promotionCreateDTO.DiscountPercent,
                Quantity = promotionCreateDTO.Quantity
            };

            await _promotionRepository.AddAsync(promotion);

            return await _promotionRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, PromotionUpdateDTO promotionUpdateDTO)
        {
            var existingPromotion = await _promotionRepository.GetByIdAsync(id);
            if (existingPromotion == null)
                throw new KeyNotFoundException($"Promotion with id '{id}' not found.");

            var duplicatePromotion = await _promotionRepository.GetByNameAsync(promotionUpdateDTO.Name);
            if (duplicatePromotion != null && duplicatePromotion.Id != id)
                throw new InvalidOperationException("A promotion with the same name already exists.");

            var now = DateTime.Now;

            if (promotionUpdateDTO.StartDate < now)
                throw new ArgumentException("StartDate must be in the future.");

            if (promotionUpdateDTO.EndDate <= now)
                throw new ArgumentException("EndDate must be in the future.");

            if (promotionUpdateDTO.StartDate >= promotionUpdateDTO.EndDate)
                throw new ArgumentException("StartDate must be earlier than EndDate.");

            if (promotionUpdateDTO.Condition < 0)
                throw new ArgumentException("Condition must be non-negative.");

            if (promotionUpdateDTO.DiscountPercent <= 0 || promotionUpdateDTO.DiscountPercent > 100)
                throw new ArgumentException("DiscountPercent must be between 1 and 100.");

            if (promotionUpdateDTO.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");

            existingPromotion.Name = promotionUpdateDTO.Name;
            existingPromotion.StartDate = promotionUpdateDTO.StartDate;
            existingPromotion.EndDate = promotionUpdateDTO.EndDate;
            existingPromotion.Condition = promotionUpdateDTO.Condition;
            existingPromotion.DiscountPercent = promotionUpdateDTO.DiscountPercent;
            existingPromotion.Quantity = promotionUpdateDTO.Quantity;
            existingPromotion.IsDeleted = promotionUpdateDTO.IsDeleted;

            _promotionRepository.Update(existingPromotion);

            return await _promotionRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingPromotion = await _promotionRepository.GetByIdAsync(id);
            if (existingPromotion == null)
                throw new KeyNotFoundException($"Promotion with id '{id}' not found.");

            if (existingPromotion.IsDeleted == true)
                throw new InvalidOperationException($"Promotion with id {id} is already deleted.");

            _promotionRepository.Delete(existingPromotion);

            return await _promotionRepository.SaveChangesAsync();
        }
    }
}
