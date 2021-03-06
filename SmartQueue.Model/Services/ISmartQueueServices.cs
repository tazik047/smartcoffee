﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Services
{
    public interface ISmartQueueServices
    {
        IUserService UserService { get; }

        IPreferencesService PreferencesService { get; }

        IRoleService RoleService { get; }

        ICompanyService CompanyService { get; }

        ICoffeeMachineService CoffeeMachineService { get; }

        IQueueService QueueService { get; }
    }
}
