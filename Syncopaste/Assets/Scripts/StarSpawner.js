#pragma strict

public var prefabs: UnityEngine.GameObject[];
public var delay: float = 2;
public var delayRange: Vector2 = Vector2(0.2, 2);
public var active: int = 1;
private var screenUnitWidth: float;

function Start () {
	screenUnitWidth = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2;
	ResetDelay();
	StartCoroutine(StarGenerator());
}

function StarGenerator (): IEnumerator {
	yield WaitForSeconds (delay);
	if (active) {
		var newTransform = transform;
		newTransform.position.x = Random.Range(-screenUnitWidth, screenUnitWidth);
		Instantiate(prefabs[Random.Range(0, prefabs.Length)], newTransform.position, Quaternion.identity);
		ResetDelay();
	}
	
	StartCoroutine(StarGenerator());
}

function ResetDelay () {
	 delay = Random.Range(delayRange.x, delayRange.y);
}