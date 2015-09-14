using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	public void HandleCollision(GameObject obj1, Collision2D col) {

		CollidableObjectModel mod1 = obj1.GetComponent<CollidableObjectModel> ();
		GameObject obj2 = col.gameObject;
		CollidableObjectModel mod2 = obj2.GetComponent<CollidableObjectModel> ();

		if (mod1 && mod2) {
			if (mod2.collidableType == CollidableType.PlayerProjectile) {
				if (mod1.collidableType == CollidableType.Enemy) {
					if (mod1.beatType == mod2.beatType)
						GameObjectUtil.Destroy(obj1);
					GameObjectUtil.Destroy(obj2);
				}
			}
		}
	}
}
