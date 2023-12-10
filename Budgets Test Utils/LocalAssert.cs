using FluentAssertions;
using FluentAssertions.Json;
using ImpromptuInterface;
using Newtonsoft.Json.Linq;

namespace Krypton.Budgets.Tests.Utils;

public class LocalAssert
{
    public static void AreEquivalent(object? expected, object? actual)
    {
        UpcastArg(actual).Should().BeEquivalentTo(UpcastArg(expected));
    }

    private static JToken UpcastArg(object? arg)
    {
        return arg switch
        {
            null => JToken.FromObject(new { }),
            JToken token => token,
            IActLikeProxy proxy => JToken.FromObject(proxy.Original),
            string str => JToken.Parse(str),
            IEnumerable<object> coll => JToken.FromObject(coll.Select(o => UpcastArg(o))),
            object obj => JToken.FromObject(obj)
        };
    }
}
