using UnityEngine;
using System.Collections;

public enum CollidableType {
	Unknown = 0,
	Player,
	Enemy,
	PlayerProjectile,
	EnemyProjectile
}

public class CollidableObjectModel : MonoBehaviour {

	public CollidableType collidableType = CollidableType.Unknown;
	public SongData.BeatType beatType = SongData.BeatType.None;
}
