namespace TodoListApp.Api.Models
{
	public class RefreshToken
	{
		public RefreshToken(string token)
		{
			Token = token;
		}
		public string Token { get; }
	}
}
