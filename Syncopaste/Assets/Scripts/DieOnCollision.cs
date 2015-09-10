using UnityEngine;
using System.Collections;

public class DieOnCollision : MonoBehaviour {

	public delegate void OnDeath();
	public event OnDeath DeathCallback;

	void OnCollisionEnter2D (Collision2D col) {
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), col.collider);

		if (DeathCallback != null) {
			DeathCallback();
		}
	}
}
