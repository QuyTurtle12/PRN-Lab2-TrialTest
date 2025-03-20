using BusinessLogic.IService;
using Data.PaggingItem;
using DataAccess.Entities;
using DataAccess.IRepository;

namespace BusinessLogic.Service
{
    public class MedicineService : IMedicineService
    {
        private readonly IUOW _unitOfWork;

        public MedicineService(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<MedicineInformation>> GetAllMedicine(int index, int pageSize, string? nameSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new Exception("Please input index or page size correctly!");
            }

            IQueryable<MedicineInformation> query = _unitOfWork.GetRepository<MedicineInformation>().Entities;

            // Search by name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(m => m.MedicineName.Contains(nameSearch));
            }

            PaginatedList<MedicineInformation> paginatedList = await _unitOfWork.GetRepository<MedicineInformation>().GetPagging(query, index, pageSize);

            return paginatedList;
        }
    }
}
