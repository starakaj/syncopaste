using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmfLite;

public class OrbitalCenter : MonoBehaviour {

	public GameObject projectilePrefab;
	public GameObject[] orbitalPrefabs;
	private GameObject[] orbitals;
	public float radius;
	public bool doRotate;
	public float phaseOffset = Mathf.PI / 2.0f;
	private float phase;
	public float rate; // cycles per second

	private byte onBeatNote = 36;
	private byte offBeatNote = 43;

	public void OnEnable() {
		BeatSynchronizer.OnAudioStart += StartRotation;
	}

	public void OnDisable() {
		BeatSynchronizer.OnAudioStart -= StartRotation;
	}

	public void StartRotation(double syncTime, double lookahead) {
		phase = (2f *  Mathf.PI) * (float) (lookahead * rate) + phaseOffset;

		float currentTime = (float)AudioSettings.dspTime;

		StartCoroutine (StartRotationWithDelay ((float)syncTime - currentTime));
	}

	IEnumerator StartRotationWithDelay(float delay) {
		yield return new WaitForSeconds(delay);

		doRotate = true;
	}

	public void SetBPM(float bpm, int beatsPerCycle) {
		var period = beatsPerCycle * 60f / bpm;
		if (period > 0)
			rate = 1.0f/period;
	}

	private int IndexOfActiveOrbital() {
		float phaseBinWidth = (2.0f * Mathf.PI / orbitals.Length);
		float wrappedPhase = (phase + 2.0f * Mathf.PI - phaseOffset) % (2.0f * Mathf.PI);
		float phaseBin = wrappedPhase / phaseBinWidth;
		int idx = Mathf.RoundToInt (phaseBin) % orbitals.Length;

		return idx;
	}

	private void RepositionOrbitalsForCurrentPhase() {

		if (orbitalPrefabs.Length == 0)
			return;

		float da = (float) (-2.0 * Mathf.PI / orbitalPrefabs.Length);
		float pixelRadius = radius;
		for (int i=0; i<orbitalPrefabs.Length; ++i) {
			float angle = phase + (da * i);
			var px = pixelRadius * Mathf.Cos (angle);
			var py = pixelRadius * Mathf.Sin (angle);

			var pos = transform.position;
			pos.x += px;
			pos.y += py;

			orbitals[i].transform.position = pos;
		}
	}

	void Start () {

		orbitals = new GameObject[orbitalPrefabs.Length];
		for (int i=0; i<orbitalPrefabs.Length; ++i) {
			SongData.BeatType beatType = (i%2 == 0) ? SongData.BeatType.OnBeat : SongData.BeatType.OffBeat;
			orbitals[i] = GameObjectUtil.Instantiate(orbitalPrefabs[i], Vector3.zero);
			Color c = ShipViewModel.ColorForBeatType(beatType);
			orbitals[i].GetComponent<SpriteRenderer>().material.color = c;
		}

		RepositionOrbitalsForCurrentPhase ();
	}

	// Update is called once per frame
	void Update () {

		RepositionOrbitalsForCurrentPhase();

		if (doRotate) {
			phase += (Time.deltaTime * rate * (float) Mathf.PI * 2.0f);
			phase %= (2.0f * Mathf.PI);
		}

		if (Input.GetButtonDown("Fire1")) {
			
			MidiEvent? e = GameObject.Find("GameManager").GetComponent<EventStore>().GetCurrentMidiEvent();
			
			if (e.HasValue) {
				SongData.BeatType beatType = SongData.BeatType.None;
				if (e.Value.data1 == onBeatNote)
					beatType = SongData.BeatType.OnBeat;
				else if (e.Value.data1 == offBeatNote) 
					beatType = SongData.BeatType.OffBeat;

				var pos = gameObject.transform.position;
				pos.y += radius;

				GameObject projectile = GameObjectUtil.Instantiate(projectilePrefab, pos);
				projectile.GetComponent<CollidableObjectModel>().beatType = beatType;
				Color c = ShipViewModel.ColorForBeatType(beatType);
				projectile.GetComponent<SpriteRenderer>().material.color = c;

				int orbitalIndex = IndexOfActiveOrbital();
				orbitals[orbitalIndex].SetActive(false);

				float delayTime = (rate == 0f) ? 0.5f : (0.5f / rate); 
				StartCoroutine(ReactivateOrbitalAfterDelay(orbitalIndex, delayTime));
			}
		}
	}

	IEnumerator ReactivateOrbitalAfterDelay(int orbital, float delayInSeconds) {
		yield return new WaitForSeconds(delayInSeconds);
		orbitals [orbital].SetActive (true);
	}
}
