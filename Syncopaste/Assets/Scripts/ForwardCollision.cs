using UnityEngine;
using System.Collections;

public class ForwardCollision : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D col) {

		CollisionManager manager = GameObject.Find ("GameManager").GetComponent<CollisionManager> ();

		manager.HandleCollision (gameObject, col);
	}
}
