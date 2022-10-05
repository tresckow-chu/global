// AuthModel.cs
using System.ComponentModel.DataAnnotations;

namespace BPSGobalClient.Entities
{
	public class LoginResult
	{
		public string UserName { get; set; }
		public JwtToken Token { get; set; }

		public string Message { get; set; }
		public string Email { get; set; }
		//public string jwtBearer { get; set; }
		
		public bool IsSuccess { get; set; }
	}
	public class LoginModel
	{
		[Required(ErrorMessage = "Email is required.")]
		//[EmailAddress(ErrorMessage = "Email address is not valid.")]
		public string UserName { get; set; } // NOTE: email will be the username, too

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		
		//[EmailAddress(ErrorMessage = "Email address is not valid.")]
		public string PrimaryEmailAddress { get; set; } 



	}


	public class RegModel : LoginModel
	{
		[Required(ErrorMessage = "Confirm password is required.")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
		public string confirmpwd { get; set; }
	}
}
