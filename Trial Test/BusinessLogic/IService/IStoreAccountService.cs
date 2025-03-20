using DataAccess.Entities;

namespace BusinessLogic.IService
{
    public interface IStoreAccountService
    {
        Task<StoreAccount> Login(string email, string password);
    }
}
