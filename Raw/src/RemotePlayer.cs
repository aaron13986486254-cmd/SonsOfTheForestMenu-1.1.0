using UnityEngine;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("roster row")]
public struct RemotePlayer
{
	public string Name;

	public ulong SteamId;

	public Vector3 Position;

	public float Health;

	public float MaxHealth;

	public bool HasHealth;

	public bool IsAlive;

	public bool IsLocal;

	public bool IsAdmin;

	public BoltEntity Entity;
}
