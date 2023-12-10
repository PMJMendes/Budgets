using ImpromptuInterface;
using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Global.Echo;
using Krypton.Budgets.Tests.Domain._Base;
using Krypton.Budgets.Tests.Utils;

namespace Krypton.Budgets.Tests.Domain.Global;

public class EchoTest : TestBase<IEcho>
{
    private readonly Guid loggedId = Guid.NewGuid();
    private readonly string externId = Guid.NewGuid().ToString();

    public EchoTest()
    {
        SetupTestData("User", new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                Email = "another@domain.invalid",
                ExternalId = Guid.NewGuid().ToString(),
                FullName = "Another User"
            },
            new
            {
                Id = loggedId,
                Email = "user@domain.invalid",
                ExternalId = externId,
                FullName = "Domain User"
            }
        }); ;
    }

    [Fact]
    public async Task TestNormalOperation()
    {
        SetupUser(false, loggedId, externId, "Domain User(user@domain.invalid)");

        var expected = new[]
        {
            new
            {
                Reply = "Domain User (user@domain.invalid)"
            }
        };

        var called = false;
        var actual = await subject.Fetch(new object().ActLike<IArguments>(), results =>
        {
            called = true;
            return results;
        }).ToListAsync();

        persistence.VerifyAll();

        Assert.True(called);
        LocalAssert.AreEquivalent(expected, actual);
    }

    [Fact]
    public async Task TestUserIsKeycloakOnly()
    {
        SetupUser(true, null, Guid.NewGuid().ToString(), "swagger");

        var expected = new[]
        {
            new
            {
                Reply = "swagger"
            }
        };

        var called = false;
        var actual = await subject.Fetch(new object().ActLike<IArguments>(), results =>
        {
            called = true;
            return results;
        }).ToListAsync();

        persistence.VerifyAll();

        Assert.True(called);
        LocalAssert.AreEquivalent(expected, actual);
    }
}
