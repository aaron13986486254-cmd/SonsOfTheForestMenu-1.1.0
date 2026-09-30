namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x]
internal static class Signature
{
    internal const string Author="cyberfox1337x";
    internal const string Guid="com.stormfest.sonsoftheforestmenu";
    internal const string Name="Sons of the Forest Menu";
    internal const string Version="1.2.1";
    internal const string VerifiedGameBuild="0.11.3";
    internal static string HeaderLine=>$"SOTF UTIL  ·  v{Version}  ·  GAME {VerifiedGameBuild}";
    internal static string BootLine(string toggleKey)=>$"{Name} v{Version} by {Author} — press {toggleKey} to open.";
}