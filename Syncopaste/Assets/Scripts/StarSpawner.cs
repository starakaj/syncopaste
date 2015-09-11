using UnityEngine;
using System.Collections;

public class StarSpawner : MonoBehaviour {

	public GameObject[] prefabs;
	public CollisionManager collisionDelegate;
	public float delay = 2f;
	public Vector2 delayRange = new Vector2(.2f, 2);
	public bool active = true;

	private float screenUnitWidth;

	// Use this for initialization
	void Start () {
		screenUnitWidth = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2;
		ResetDelay();
		StartCoroutine(StarGenerator());
	}

	IEnumerator StarGenerator () {
		yield return new WaitForSeconds (delay);
		if (active) {
			var newTransform = transform;
			var position = newTransform.position;
			position.x = Random.Range(-screenUnitWidth, screenUnitWidth);
			newTransform.position = position;

			GameObjectUtil.Instantiate(prefabs[Random.Range(0, prefabs.Length)], newTransform.position);

			ResetDelay();
		}
		
		StartCoroutine(StarGenerator());
	}

	void ResetDelay() {
		delay = Random.Range(delayRange.x, delayRange.y);
	}
}
