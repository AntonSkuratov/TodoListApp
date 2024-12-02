namespace TodoListApp.Api.Models
{
	//Класс для хранения access- и refresh- токенов
	public class TokenModel
	{
		public TokenModel(string accessToken, string refreshToken)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}
		public string AccessToken { get; }
		public string RefreshToken { get; }
	}
}
