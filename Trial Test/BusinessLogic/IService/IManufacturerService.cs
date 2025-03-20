using DataAccess.Entities;

namespace BusinessLogic.IService
{
	public interface IManufacturerService
	{
		Task<ICollection<Manufacturer>> GetManufacturers();
	}
}
