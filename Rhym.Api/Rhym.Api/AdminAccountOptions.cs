using System.ComponentModel.DataAnnotations;

namespace Rhym.Api;

public class AdminAccountOptions
{
	public const string SectionName = "AdminAccount";

	[Required]
	public required string Email { get; set; }

	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public DateTime Birthday { get; set; }

	[Required]
	public required string Password { get; set; }
}
