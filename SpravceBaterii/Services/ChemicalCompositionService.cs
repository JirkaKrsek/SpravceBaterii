using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class ChemicalCompositionService
    {
        private readonly ApplicationDbContext ApplicationDbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public ChemicalCompositionService(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Načtení všech chemických složení
        /// </summary>
        /// <returns>List chemických složení</returns>
        public async Task<List<ChemicalComposition>> GetChemicalCompositions()
        {
            return await ApplicationDbContext.ChemicalCompositions.AsNoTracking().ToListAsync();
        }
    }
}
