using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;

	private GameObject player;
	private StarSpawner starSpawner;
	private bool isGameRunning;
	private BeatSynchronizer synchronizer;

	void Awake () {
		starSpawner = GameObject.Find ("StarSpawner").GetComponent<StarSpawner> ();
		synchronizer = GameObject.Find ("Main Camera").GetComponent<BeatSynchronizer> ();
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

		synchronizer.Play ();
		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback += GameOver;
	}

	void GameOver () {
		isGameRunning = false;
		starSpawner.active = false;

		synchronizer.Stop ();
		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback -= GameOver;
		GameObjectUtil.Destroy (player);
	}
}
