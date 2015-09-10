using UnityEngine;
using System.Collections;

public class KeyboardSpaceFlight : MonoBehaviour {

	public float speed = 40f;
	public int offscreenPx = 10;
	public float tilt = 10f;

	private float offscreenX, offscreenY;

	// Use this for initialization
	void Start () {
		offscreenX = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2 + offscreenPx;
		offscreenY = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + offscreenPx;
	}
	
	// Update is called once per frame
	void Update () {
		var velocity = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
		velocity *= speed * Time.deltaTime;

		{
			var position = transform.position;
			position += velocity;
			position.x = Mathf.Clamp(position.x, -offscreenX, offscreenX);
			position.y = Mathf.Clamp(position.y, -offscreenY, offscreenY);
			transform.position = position;
		}

		{
			var rotation = transform.rotation;
			rotation.z = Input.GetAxis("Horizontal") * tilt;
		}
	}
}