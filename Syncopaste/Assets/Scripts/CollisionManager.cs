using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	public void HandleCollision(GameObject obj, Collision2D col) {

		ShipState state1 = obj.GetComponent<ShipState> ();
		ShipState state2 = col.gameObject.GetComponent<ShipState> ();

		if (state1 != null && state2 != null) {
			if (state1.beatType == state2.beatType) {
				GameObjectUtil.Destroy(obj);
			}

			GameObjectUtil.Destroy(col.gameObject);
		}
	}
}
