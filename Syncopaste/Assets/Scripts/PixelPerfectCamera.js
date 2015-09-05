#pragma strict

public static var pixelsToUnits: float = 1;
public static var scale: float = 1;

public var nativeResolution: Vector2 = Vector2(240, 160);

function Awake () {
	var camera = Camera.main;
	if (camera.orthographic) {
		scale = Screen.height / nativeResolution.y;
		pixelsToUnits *= scale;
		camera.orthographicSize = (Screen.height / 2) / pixelsToUnits;
	}
}

function Start () {

}

function Update () {

}