// ReSharper disable once CheckNamespace

using Orders.Core.Extensions;

// ReSharper disable once CheckNamespace
namespace MongoDB.Bson.Serialization.Conventions;

/// <summary>
///     A convention that sets the element name to the snake-cased version of member name.
/// </summary>
public class SnakeCaseElementNameConvention : ConventionBase, IMemberMapConvention
{
    // public methods
    /// <summary>
    ///     Applies a modification to the member map.
    /// </summary>
    /// <param name="memberMap">The member map.</param>
    public void Apply(BsonMemberMap memberMap)
    {
        var name = memberMap.MemberName;
        name = GetElementName(name);
        memberMap.SetElementName(name);
    }

    // private methods
    private string GetElementName(string memberName)
    {
        return memberName.Length switch
        {
            0 => "",
            // 1 => Char.ToLowerInvariant(memberName[0]).ToString(),
            _ => memberName.ToSnakeCase()
        };
    }
}