namespace TwoFactorAuthentication.Authentication.Models
{
	public class ResponseModel<T> where T : class
	{
		public T? Data { get; set; }
		public bool IsSuccessed { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
