using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Queries.Authentication.Me;

public sealed class GetCurrentUserQueryHandler(IAuthenticationService authenticationService) : IRequestHandler<GetCurrentUserQuery, ResponseEntity<ApplicationUser>>
{
    private readonly IAuthenticationService  _authenticationService = authenticationService;
    
    public async Task<ResponseEntity<ApplicationUser>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.GetCurrentUserAsync(request, cancellationToken);
        var responseEntity = ResponseEntity<ApplicationUser>.Succeeded();
        return responseEntity.WithData(result);
    }
}