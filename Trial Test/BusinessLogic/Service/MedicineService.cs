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
			await _unitOfWork.GetRepository<MedicineInformation>().InsertAsync(medicineInformation);
            await _unitOfWork.SaveAsync();
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

            PaginatedList<MedicineInformation> paginatedList = await _unitOfWork.GetRepository<MedicineInformation>().GetPagging(query, index, pageSize);

            return paginatedList;
        }

		public async Task UpdateMedicine(MedicineInformation updatedMedicineInformation)
		{
            MedicineInformation? medicineInformation = _unitOfWork.GetRepository<MedicineInformation>().GetById(updatedMedicineInformation.MedicineId);

            medicineInformation = updatedMedicineInformation;

            await _unitOfWork.GetRepository<MedicineInformation>().UpdateAsync(medicineInformation);
			await _unitOfWork.SaveAsync();
		}
	}
}
