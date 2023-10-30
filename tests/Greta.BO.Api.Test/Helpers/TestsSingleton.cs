using AutoMapper;
using Greta.Sdk.EFCore.Middleware;

namespace Greta.BO.Api.Test.Helpers;

public class TestsSingleton
{
    private static IMapper _mapper;

    public static IMapper Mapper
    {
        get
        {
            if (_mapper == null)
            {
                // Auto Mapper Configurations
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new TestMappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            return _mapper;
        }
    }

    private static AuthenticateUser<string> _auth;

    public static AuthenticateUser<string> Auth
    {
        get
        {
            if (_auth == null)
            {
                _auth = new()
                {
                    UserName = "Greta.BO.Api",
                    UserId = new Guid().ToString(),
                    IsAuthenticated = true,
                    IsApplication = false,
                };
            }

            return _auth;
        }
    }
}