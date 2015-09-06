#pragma strict

public var type: int;

function Start () {

}

function Update () {

}

public function Trigger () {
	var size = Random.Range(0.2, 5.0);
	transform.localScale = Vector3(size, size, 1);
}