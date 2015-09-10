using UnityEngine;
using System.Collections;

public class ProjectileShooter : MonoBehaviour {
	
	public GameObject projectilePrefab;

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			var projectileTransform = transform;
			GameObject projectile = GameObjectUtil.Instantiate(projectilePrefab, projectileTransform.position);
			var rcolor = new Color(Random.Range(0.2f, 1.0f), Random.Range(0.2f, 1.0f), Random.Range(0.2f, 1.0f), 1.0f);
			projectile.GetComponent<SpriteRenderer> ().color = rcolor;
		}
	}
}
