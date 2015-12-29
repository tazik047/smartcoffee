using System.Linq;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    class PreferencesService : IPreferencesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PreferencesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CoffeePreferences GetUserPreferences(User user)
        {
            var preferences = _unitOfWork.CoffeePreferencesRepository
                .Get(p => p.Id == user.Id)
                .FirstOrDefault() ?? new CoffeePreferences
                {
                    Drink = DrinkType.Coffee,
                    Size = Size.Medium,
                    Sugar = 2
                };

            return preferences;
        }

        public void UpdateUserPreferences(User user, CoffeePreferences preferences)
        {
            var originPreferneces = _unitOfWork.CoffeePreferencesRepository
                .Get(p => p.Id == preferences.Id).FirstOrDefault();
            preferences.Id = user.Id;
            if (originPreferneces == null)
            {
                _unitOfWork.CoffeePreferencesRepository.Add(preferences);
            }
            else
            {
                _unitOfWork.CoffeePreferencesRepository.Edit(preferences);
            }
            _unitOfWork.Save();
        }
    }
}
