using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject startText;

	private GameObject player;
	private StarSpawner starSpawner;
	private bool isGameRunning;
	private BeatSynchronizer synchronizer;

	private void SetIsGameRunning(bool running) {
		isGameRunning = running;
		Color c = startText.GetComponent<SpriteRenderer> ().material.color;
		c.a = running ? 0 : 1;
		startText.GetComponent<SpriteRenderer> ().material.color = c;
		starSpawner.active = running;
	}

	void Awake () {
		starSpawner = GameObject.Find ("StarSpawner").GetComponent<StarSpawner> ();
		synchronizer = GameObject.Find ("Main Camera").GetComponent<BeatSynchronizer> ();
	}

	void Start () {
		SetIsGameRunning (false);
		isGameRunning = false;
		Color c = startText.GetComponent<SpriteRenderer> ().material.color;
		c.a = 1;
		startText.GetComponent<SpriteRenderer> ().material.color = c;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isGameRunning) {
			if (Input.GetButtonDown("Fire1")) {
				BeginGame();
			}
		}
	}

	void BeginGame () {
		SetIsGameRunning (true);

		player = GameObjectUtil.Instantiate (playerPrefab, Vector2.zero);

		synchronizer.Play ();
		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback += GameOver;
	}

	void GameOver () {
		SetIsGameRunning (false);

		synchronizer.Stop ();
		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback -= GameOver;
		GameObjectUtil.Destroy (player);
	}
}
