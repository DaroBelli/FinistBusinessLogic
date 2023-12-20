using FinistBusinessLogic.Context;
using Grpc.Core;

namespace FinistBusinessLogic.Services
{
    public class UserApiService : UserService.UserServiceBase
    {
        /// <summary>
        /// Находит все данные по пользователю.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<UserReply> GetUser(PhoneNumber phoneNumber, ServerCallContext context)
        {
            var user = new UserReply();

            try
            {
                using var db = new PostgresContext();

                var userDB = db.Clients
                    .Where(x => x.PhoneNumber == phoneNumber.PhoneNumber_)
                    .Select(x => new UserReply
                    {
                        FirstName = x.FirstName,
                        PhoneNumber = x.PhoneNumber,
                        LastName = x.LastName,
                        MiddleName = x.MiddleName,
                    })
                    .FirstOrDefault();

                if (userDB != null)
                {
                    userDB.BankAccounts.AddRange(GetBankAccounts(userDB.PhoneNumber, db).Result);

                    user = userDB;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            return Task.FromResult(user);
        }

        /// <summary>
        /// Находит данные по счетам пользователя.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя</param>
        /// <param name="db">Контекст БД.</param>
        /// <returns></returns>
        private static Task<List<BankAccountReply>> GetBankAccounts (string phoneNumber, PostgresContext db)
        {
            return Task.FromResult(db.BankAccounts
                        .Where(x => x.ClientPhoneNumber == phoneNumber)
                        .Select(x => new BankAccountReply
                        {
                            AccountNumber = x.AccountNumber,
                            AccountType = x.AccountType,
                            PhoneNumber = x.ClientPhoneNumber
                        })
                        .ToList());
        }
    }
}
