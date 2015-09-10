using UnityEngine;
using System.Collections;

public class DestroyOffscreen : MonoBehaviour {
	
	public float offset = 16f;
	public delegate void OnDestroy();
	public event OnDestroy DestroyCallback;
	
	private bool isOffscreen;
	private float offscreenX = 0f;
	private float offscreenY = 0f;
	private Rigidbody2D body2d;
	
	void Awake () {
		body2d = GetComponent<Rigidbody2D> ();
	}
	
	void Start () {
		offscreenX = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2 + offset;
		offscreenY = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + offset;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = transform.position;
		var dir = body2d.velocity;
		isOffscreen = false;

		if (Mathf.Abs (pos.x) > offscreenX) {
			if (dir.x < 0 && pos.x < -offscreenX)
				isOffscreen = true;
			else if (dir.x > 0 && pos.x > offscreenX)
				isOffscreen = true;
		} else if (Mathf.Abs (pos.y) > offscreenY) {
			if (dir.y < 0 && pos.y < -offscreenY)
				isOffscreen = true;
			else if (dir.y > 0 && pos.y > offscreenY)
				isOffscreen = true;
		}
		
		if (isOffscreen) {
			OnOutOfBounds();
		}
	}
	
	public void OnOutOfBounds() {
		isOffscreen = false;
		GameObjectUtil.Destroy (gameObject);
		
		if (DestroyCallback != null) {
			DestroyCallback();
		}
	}
}
