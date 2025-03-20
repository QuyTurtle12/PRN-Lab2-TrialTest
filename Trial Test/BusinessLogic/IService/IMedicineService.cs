using Data.PaggingItem;
using DataAccess.Entities;

namespace BusinessLogic.IService
{
    public interface IMedicineService
    {
        public Task<PaginatedList<MedicineInformation>> GetAllMedicine(int index, int pageSize, string? nameSearch);
    }
}
