using System.Linq;
using AutoMapper;
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
            preferences.Id = user.Id;
            var originPreferneces = _unitOfWork.CoffeePreferencesRepository
                .Get(p => p.Id == preferences.Id).FirstOrDefault();
            if (originPreferneces == null)
            {
                _unitOfWork.CoffeePreferencesRepository.Add(preferences);
            }
            else
            {
                originPreferneces.CoffeeMachine = null;
                originPreferneces.CoffeeMachineId = preferences.CoffeeMachineId;
                originPreferneces.Size = preferences.Size;
                originPreferneces.Drink = preferences.Drink;
                originPreferneces.Sugar = preferences.Sugar;
                _unitOfWork.CoffeePreferencesRepository.Edit(originPreferneces);
            }
            _unitOfWork.Save();
        }
    }
}
