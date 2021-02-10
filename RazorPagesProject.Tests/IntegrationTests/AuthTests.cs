using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesProject.Tests
{
    public class AuthTests :
        IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup>
            _factory;

        public AuthTests(
            CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser()
        {
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var response = await client.GetAsync("/SecurePage");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login",
                response.Headers.Location.OriginalString);
        }

        

        [Fact]
        public async Task Get_securePageIsReturnedForAnAuthenticateUser()
        {
            var client = _factory.WithWebHostBuilder(x =>
                {
                    x.ConfigureTestServices(s =>
                    {
                        s.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", o => { });
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Test");

            var response = await client.GetAsync("/SecurePage");

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }


        public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder,
                ISystemClock clock
            ) : base(options, logger, encoder, clock)
            {

            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Test user"),
                };

                var identity = new ClaimsIdentity(claims, "Test");
                var principle = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principle, "Test");

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }

    }
}
