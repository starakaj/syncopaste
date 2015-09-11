using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShipState))]
public class ProjectileShooter : MonoBehaviour {
	
	public GameObject projectilePrefab;

	// Update is called once per frame
	void Update () {

		ShipState state = GetComponent<ShipState> ();

		if (Input.GetButtonDown ("Fire1")) {
			var projectileTransform = transform;
			GameObject projectile = GameObjectUtil.Instantiate(projectilePrefab, projectileTransform.position);

			var rcolor = state.ColorForBeatType(state.beatType);

			projectile.GetComponent<SpriteRenderer> ().color = rcolor;
			projectile.GetComponent<ShipState>().beatType = state.beatType;
		}
	}
}
