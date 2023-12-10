using ImpromptuInterface;
using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Global.Echo;
using Krypton.Budgets.Tests.API._Base;
using Krypton.Budgets.Tests.API._Impl;
using Krypton.Budgets.Tests.Utils;
using Moq;

namespace Krypton.Budgets.Tests.API.Global;

public class EchoTest : TestBase
{
    private readonly Mock<IEcho> query;
    private readonly object testArgs = new();

    public EchoTest() : base(new[] { new Mock<IEcho>(MockBehavior.Strict) })
    {
        query = GetMock<IEcho>();
    }

    [Fact]
    public async Task TestNormalOperation()
    {
        var reply = "Echo test reply";
        var qReply = new[]
        {
            new
            {
                Reply = reply
            }.ActLike<IEchoItem>()
        };

        var expected = new[]
        {
            new
            {
                reply
            }
        };

        SetupQueryCall(query, testArgs.ActLike<IArguments>(), qReply);

        var actual = await client.GetAsync("/global/Echo").
            ParseResponse();

        query.VerifyAll();

        LocalAssert.AreEquivalent(expected, actual);
    }
}
