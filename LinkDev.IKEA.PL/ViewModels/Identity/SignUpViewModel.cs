﻿using System.ComponentModel.DataAnnotations;

namespace LinkDev.IKEA.PL.ViewModels.Identity
{
	public class SignUpViewModel
	{
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = null!;
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "User Name Is Required ya prince")]
        public string UserName { get; set; } = null!;
		[EmailAddress]
        public string Email { get; set; } = null!;
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		[Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Confirmed Password doesn't match with password")]
        public string ConfirmPassword { get; set; } = null!;
		public bool IsAgree { get; set; }
    }
}
