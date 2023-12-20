using FinistBusinessLogic.Context;
using FinistBusinessLogic.Controllers;
using Grpc.Core;

namespace FinistBusinessLogic.Services
{
    public class LoginningApiService : LoginningService.LoginningServiceBase
    {
		/// <summary>
		/// Проверяет, совпадают ли телефон и пароль с данными из БД.
		/// </summary>
		/// <param name="loginInfo">Данные для входа.</param>
		/// <returns>Возвращает true, если данные совпадают и не возникло никаких ошибок, иначе false.</returns>
        public override Task<IsCorrectLoginInfo> TryLoginning(LoginInfo loginInfo, ServerCallContext context)
        {
			IsCorrectLoginInfo isCorrectLoginInfo = new()
			{
				IsCorrect = false
			};

			try
			{
				using var db = new PostgresContext();

				string encryptingPassword = Encrypting.Encrypt(loginInfo.Password);

				var user = db.Clients
					.Where(x => x.PhoneNumber == loginInfo.PhoneNumber)
					.FirstOrDefault(x => x.Pass == encryptingPassword);

				if (user != null)
				{
					isCorrectLoginInfo.IsCorrect = true;
				}
			}
			catch (Exception e)
			{
                Console.WriteLine(e.Message);
            }

			return Task.FromResult(isCorrectLoginInfo);
        }
    }
}
