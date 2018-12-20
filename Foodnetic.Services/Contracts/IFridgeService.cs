﻿using System.Collections.Generic;
using Foodnetic.Models;
using Foodnetic.ViewModels.Groceries;

namespace Foodnetic.Services.Contracts
{
    public interface IFridgeService
    {
        void CreateGrocery(CreateGroceryViewModel bindingModel, string userId);
        IEnumerable<Grocery> GetAllGroceries(string name);
    }
}
