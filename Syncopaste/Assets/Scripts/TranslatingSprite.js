#pragma strict

public var velocity: Vector2 = Vector2.zero;

function Update () {
	transform.position += velocity * Time.deltaTime;
}