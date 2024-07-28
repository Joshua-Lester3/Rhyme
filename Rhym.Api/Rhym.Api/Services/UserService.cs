using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rhym.Api.Data;
using Rhym.Api.Dtos;
using Rhym.Api.Models;

namespace Rhym.Api.Services;

public class UserService
{
	private readonly AppDbContext _context;
	private readonly UserManager<AppUser> _userManager;

	public UserService(AppDbContext context, UserManager<AppUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	public async Task<RegistrationDto> AddUser(UserDto userDto)
	{
		if (await _userManager.FindByEmailAsync(userDto.Email) == null && await _userManager.FindByNameAsync(userDto.Username) == null)
		{
			AppUser user = new AppUser
			{
				Email = userDto.Email,
				UserName = userDto.Username
			};

			IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);

			if (result.Succeeded)
			{
				return new RegistrationDto() { Results = RegistrationResults.Success };
			} else
			{
				return new RegistrationDto { Results = RegistrationResults.Failure, Errors = result.Errors };
			}

		}
		return new RegistrationDto { Results = RegistrationResults.AccountExists };
	}

	public async Task<ProfileInfoDto?> GetProfileInfo(string userId)
	{
		var foundUser = await _userManager.FindByIdAsync(userId);
		if (foundUser is null)
		{
			return null;
		}

		return new ProfileInfoDto
		{
			Email = foundUser.Email!,
			NumberOfDocuments = _context.Documents.Count(document => document.UserId.Equals(userId)),
		};
	}
}

public class RegistrationDto
{
    public RegistrationResults Results { get; set; }
	public IEnumerable<IdentityError>? Errors { get; set; }
}

public enum RegistrationResults
{
	Success,
	AccountExists,
	Failure,
}
