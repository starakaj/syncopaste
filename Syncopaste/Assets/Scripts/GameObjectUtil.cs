using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectUtil {
	
	private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool> ();
	
	public static GameObject Instantiate(GameObject prefab, Vector3 pos) {
		GameObject instance = null;
		
		var recycledGameObject = prefab.GetComponent<RecycleGameObject> ();
		if (recycledGameObject != null) {
			var pool = GetObjectPool (recycledGameObject);
			instance = pool.NextObject (pos).gameObject;
		} else {
			instance = GameObject.Instantiate (prefab);
			instance.transform.position = pos;
		}
		
		return instance;
	}
	
	public static void Destroy(GameObject gameObject) {
		
		var recycleGameObject = gameObject.GetComponent<RecycleGameObject> ();
		if (recycleGameObject != null) {
			recycleGameObject.Shutdown ();
		} else {
			GameObject.Destroy (gameObject);
		}
	}
	
	private static ObjectPool GetObjectPool(RecycleGameObject reference) {
		ObjectPool pool = null;
		
		if (pools.ContainsKey (reference)) {
			pool = pools [reference];
		} else {
			var poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");
			pool = poolContainer.AddComponent<ObjectPool>();
			pool.prefab = reference;
			pools.Add(reference, pool);
		}
		
		return pool;
	}
}
