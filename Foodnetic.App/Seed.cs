using System.Collections.Generic;
using Foodnetic.Data;
using Foodnetic.Models;

namespace Foodnetic.App
{
    public class Seed
    {
        private readonly FoodneticDbContext dbContext;

        public Seed(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SeedRecipes()
        {
            
        }
    }
}
