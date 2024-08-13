using E = ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ETicaretAPI.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<E.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<E.AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Username,
                Email = request.Email,
                NameSurname = request.NameSurname,
            }, request.Password);

            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (response.Succeeded)
                response.Message = "Kullanıcı kaydı yapıldı.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description} <br>";

            return response;

            //throw new UserCreateFailedException();
        }
    }
}
