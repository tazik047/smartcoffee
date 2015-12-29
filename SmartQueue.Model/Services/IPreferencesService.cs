﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Services
{
    public interface IPreferencesService
    {
        CoffeePreferences GetUserPreferences(User user);

        void UpdateUserPreferences(User user, CoffeePreferences preferences);
    }
}
