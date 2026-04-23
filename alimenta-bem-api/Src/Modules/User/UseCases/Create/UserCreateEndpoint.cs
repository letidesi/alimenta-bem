using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.User.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.Create;

public class UserCreateEndPoint : Endpoint<UserCreateRequest, UserCreateResponse, UserCreateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("user");
        Options(u => u.WithTags("user"));
        Summary(s =>
        {
            s.Summary = "Create a new user";
            s.Description = "Register a user on the platform";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserCreateRequest req, CancellationToken ct)
    {
        try
        {
            var userCreateUseCase = new UserCreateUseCase(_context, _localizer);

            var user = Map.ToEntity(req);

            var createUser = await userCreateUseCase.exec(user);

            var createdUser = Map.FromEntity(createUser);

            await SendAsync(createdUser);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}