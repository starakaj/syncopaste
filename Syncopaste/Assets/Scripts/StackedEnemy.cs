using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackedEnemy : MonoBehaviour {

	public GameObject[] slices;

	private SongData.BeatType[][] patterns;

	void Awake() {
		patterns = new SongData.BeatType[2][];
		patterns[0] = new SongData.BeatType[] {SongData.BeatType.OffBeat, SongData.BeatType.OnBeat};
		patterns[1] = new SongData.BeatType[] {SongData.BeatType.OffBeat, SongData.BeatType.SyncoBeat};

	}
	
	void Start () {
		// Give each slice in the stack a different color

		int patdex = Random.Range (0, patterns.Length);
		SongData.BeatType[] pattern = patterns [patdex];
		int offset = Random.Range (0, pattern.Length);

		for (int i=0; i<slices.Length; ++i) {
			GameObject slice = slices[i];
			SongData.BeatType beatType = pattern[(i + offset) % pattern.Length];
			Color sliceColor = ShipViewModel.ColorForBeatType(beatType);
			slice.GetComponent<MeshRenderer>().material.color = sliceColor;
			slice.GetComponent<CollidableObjectModel>().beatType = beatType;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
