using Data.PaggingItem;
using DataAccess.Entities;

namespace BusinessLogic.IService
{
    public interface IMedicineService
    {
        public Task<PaginatedList<MedicineInformation>> GetAllMedicine(int index, int pageSize, string? nameSearch);
        public Task<MedicineInformation> GetMedicine(string id);
        public Task CreateMedicine(MedicineInformation medicineInformation);
        public Task UpdateMedicine(MedicineInformation medicineInformation);
    }
}
