using BusinessLogic.IService;
using Data.PaggingItem;
using DataAccess.Entities;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
    public class MedicineService : IMedicineService
    {
        private readonly IUOW _unitOfWork;

        public MedicineService(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		public async Task CreateMedicine(MedicineInformation medicineInformation)
		{
            //check null
            if (!string.IsNullOrWhiteSpace(medicineInformation.ActiveIngredients))
            {
                // Check for special characters (#, @, &, (, ))
                if (medicineInformation.ActiveIngredients.Contains("#") ||
                    medicineInformation.ActiveIngredients.Contains("@") ||
                    medicineInformation.ActiveIngredients.Contains("&") ||
                    medicineInformation.ActiveIngredients.Contains("(") ||
                    medicineInformation.ActiveIngredients.Contains(")"))
                {
                    throw new ArgumentException("Active Ingredients cannot contain special characters like #, @, &, (, or ).");
                }

                // Check first character
                string[] words = medicineInformation.ActiveIngredients.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (string.IsNullOrEmpty(word)) continue;

                    char firstChar = word[0];
                    if (!(char.IsUpper(firstChar) || char.IsDigit(firstChar)))
                    {
                        throw new ArgumentException("Each word in Active Ingredients must start with a capital letter or a number (0-9).");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Active Ingredients cannot be empty.");
            }

            // Check length
            if (medicineInformation.ActiveIngredients.Length <= 10)
            {
                throw new ArgumentException("Active Ingredients must be greater than 10 characters.");
            }

			

            medicineInformation.MedicineId = await GenerateNewMedicineID();

			await _unitOfWork.GetRepository<MedicineInformation>().InsertAsync(medicineInformation);
            await _unitOfWork.SaveAsync();
		}

		private async Task<string> GenerateNewMedicineID()
		{
			// Fetch the latest MedicineID from the database
			MedicineInformation? latestMedicine = await _unitOfWork.GetRepository<MedicineInformation>()
				.Entities
				.OrderByDescending(m => m.MedicineId)
				.FirstOrDefaultAsync();

			string newMedicineID;
			if (latestMedicine == null)
			{
				// If no records exist, start with MI000100
				newMedicineID = "MI000100";
			}
			else
			{
				// Extract the numeric part of the latest MedicineID
				string numericPart = latestMedicine.MedicineId.Substring(2);
				int numericValue = int.Parse(numericPart);
				numericValue++;

				// Format the new MedicineID
				newMedicineID = $"MI{numericValue:D6}"; // D6 ensures 6 digits with leading zeros
			}

			return newMedicineID;
		}

		public async Task<PaginatedList<MedicineInformation>> GetAllMedicine(int index, int pageSize, string? nameSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new Exception("Please input index or page size correctly!");
            }

            IQueryable<MedicineInformation> query = _unitOfWork.GetRepository<MedicineInformation>().Entities.Include(md => md.Manufacturer);

            // Search by name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(m => m.MedicineName.Contains(nameSearch));
            }

			query = query.OrderByDescending(m => m.MedicineId);

            PaginatedList<MedicineInformation> paginatedList = await _unitOfWork.GetRepository<MedicineInformation>().GetPagging(query, index, pageSize);

            return paginatedList;
        }

		public async Task UpdateMedicine(MedicineInformation updatedMedicineInformation)
		{
            //check null
            if (!string.IsNullOrWhiteSpace(updatedMedicineInformation.ActiveIngredients))
            {
                // Check for special characters (#, @, &, (, ))
                if (updatedMedicineInformation.ActiveIngredients.Contains("#") ||
                    updatedMedicineInformation.ActiveIngredients.Contains("@") ||
                    updatedMedicineInformation.ActiveIngredients.Contains("&") ||
                    updatedMedicineInformation.ActiveIngredients.Contains("(") ||
                    updatedMedicineInformation.ActiveIngredients.Contains(")"))
                {
                    throw new ArgumentException("Active Ingredients cannot contain special characters like #, @, &, (, or ).");
                }

                // Check first character
                string[] words = updatedMedicineInformation.ActiveIngredients.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (string.IsNullOrEmpty(word)) continue;

                    char firstChar = word[0];
                    if (!(char.IsUpper(firstChar) || char.IsDigit(firstChar)))
                    {
                        throw new ArgumentException("Each word in Active Ingredients must start with a capital letter or a number (0-9).");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Active Ingredients cannot be empty.");
            }

            // Check length
            if (updatedMedicineInformation.ActiveIngredients.Length <= 10)
            {
                throw new ArgumentException("Active Ingredients must be greater than 10 characters.");
            }

            await _unitOfWork.GetRepository<MedicineInformation>().UpdateAsync(updatedMedicineInformation);
			await _unitOfWork.SaveAsync();
		}

        public async Task<MedicineInformation> GetMedicine(string id)
        {
            MedicineInformation? medicine = await _unitOfWork.GetRepository<MedicineInformation>().GetByIdAsync(id);

            return medicine;
        }
    }
}
