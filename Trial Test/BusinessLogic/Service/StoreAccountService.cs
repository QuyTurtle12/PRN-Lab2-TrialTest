using BusinessLogic.IService;
using DataAccess.Entities;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
    public class StoreAccountService : IStoreAccountService
    {
        private readonly IUOW _unitOfWork;

        public StoreAccountService(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StoreAccount> Login(string email, string password)
        {
            StoreAccount? storeAccount = await _unitOfWork.GetRepository<StoreAccount>().Entities
                                                        .Where(sa => sa.EmailAddress.Equals(email) && 
                                                                    sa.StoreAccountPassword.Equals(password))
                                                        .FirstOrDefaultAsync();

            return storeAccount;
        }
    }
}
