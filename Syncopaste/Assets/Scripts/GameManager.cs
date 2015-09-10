using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;

	private GameObject player;
	private StarSpawner starSpawner;
	private bool isGameRunning;

	void Awake () {
		starSpawner = GameObject.Find ("StarSpawner").GetComponent<StarSpawner> ();
		isGameRunning = false;
	}

	void Start () {
		BeginGame ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isGameRunning) {
			if (Input.anyKeyDown) {
				BeginGame();
			}
		}
	}

	void BeginGame () {
		isGameRunning = true;
		starSpawner.active = true;

		player = GameObjectUtil.Instantiate (playerPrefab, Vector2.zero);

		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback += GameOver;
	}

	void GameOver () {
		isGameRunning = false;
		starSpawner.active = false;

		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback -= GameOver;
		GameObjectUtil.Destroy (player);
	}
}
