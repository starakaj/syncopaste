#pragma strict

public var speed: float = 40.0;
public var offscreenPx: int = 10;
public var tilt: float = 10.0;
private var offscreenX: float = 0;
private var offscreenY: float = 0;

function Start() {
	offscreenX = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2 + offscreenPx;
	offscreenY = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + offscreenPx;
}

function Update () {
	var velocity = Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
	velocity *= speed * Time.deltaTime;
	
	transform.position += velocity;
	transform.position.x = Mathf.Clamp(transform.position.x, -offscreenX, offscreenX);
	transform.position.y = Mathf.Clamp(transform.position.y, -offscreenY, offscreenY);
	transform.rotation.z = Input.GetAxis("Horizontal") * tilt;
}