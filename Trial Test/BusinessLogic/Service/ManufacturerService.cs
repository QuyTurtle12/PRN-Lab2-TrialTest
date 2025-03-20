using BusinessLogic.IService;
using DataAccess.Entities;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
	public class ManufacturerService : IManufacturerService
	{
		private readonly IUOW _unitOfWork;

		public ManufacturerService(IUOW unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ICollection<Manufacturer>> GetManufacturers() {
			// Populate ViewBag.ManufacturerId with the list of manufacturers for the dropdown
			ICollection<Manufacturer> manufacturers = await _unitOfWork.GetRepository<Manufacturer>()
				.Entities
				.ToListAsync();

			return manufacturers;
		}
	}
}
