using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;

	private SynchronizedAudioPlayer audioPlayer;
	private GameObject player;
	private StarSpawner starSpawner;
	private bool isGameRunning;

	void Awake () {
		audioPlayer = GameObject.Find ("Main Camera").GetComponent<SynchronizedAudioPlayer> ();
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
		audioPlayer.Play ();

		player = GameObjectUtil.Instantiate (playerPrefab, Vector2.zero);

		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback += GameOver;
	}

	void GameOver () {
		isGameRunning = false;
		starSpawner.active = false;
		audioPlayer.Stop ();

		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback -= GameOver;
		GameObjectUtil.Destroy (player);
	}
}
