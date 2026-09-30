using System;

namespace Cyberfox1337x.SonsOfTheForest;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[Cyberfox1337x(null)]
internal sealed class Cyberfox1337xAttribute : Attribute
{
	public const string Handle = "cyberfox1337x";

	public string Role { get; }

	public string Bound { get; set; }

	internal Cyberfox1337xAttribute(string role = null)
	{
		Role = role;
	}

	public override string ToString()
	{
		string described = ((Role == null) ? "cyberfox1337x" : ("cyberfox1337x · " + Role));
		if (Bound != null)
		{
			return described + " · bound: " + Bound;
		}
		return described;
	}
}
