using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	public GameObject startText;
	public TextAsset levelZero;

	private GameObject player;
	private bool isGameRunning;
	private BeatSynchronizer synchronizer;
	private LevelRunner levelRunner;

	private void SetIsGameRunning(bool running) {
		isGameRunning = running;
		Color c = startText.GetComponent<SpriteRenderer> ().material.color;
		c.a = running ? 0 : 1;
		startText.GetComponent<SpriteRenderer> ().material.color = c;

		if (isGameRunning) {
			LevelModel level = new LevelModel(levelZero);
			levelRunner.SetLevel(level);
			levelRunner.Reset();
		}
	}

	void Awake () {
		synchronizer = GameObject.Find ("Main Camera").GetComponent<BeatSynchronizer> ();
		levelRunner = gameObject.GetComponent<LevelRunner> ();
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

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			GameObjectUtil.Destroy(enemy);
		}

		synchronizer.Stop ();
		var dieScript = player.GetComponent<DieOnCollision> ();
		dieScript.DeathCallback -= GameOver;
		GameObjectUtil.Destroy (player);
	}
}
