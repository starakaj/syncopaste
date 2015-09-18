using UnityEngine;
using System.Collections;

public class SynchronizedAudioSwapper : BeatEventListener {

	public AudioSource[] loops;

	private int currentLoopIndex = 0;
	private int scheduledLoopIndex = 0;

	// Use this for initialization
	void Start () {
		MixLoopsForCurrentLoopIndex ();
	}

	private void MixLoopsForCurrentLoopIndex () {
		for (int i=0; i<loops.Length; ++i) {
			loops[i].mute = (i != currentLoopIndex);
		}
	}

	override public void HandleBeatEvent(int beat, int beatsPerMeasure, float lookaheadSeconds) {
		if (beat == 0) {
			if (scheduledLoopIndex != currentLoopIndex) {
				StartCoroutine (UpdateLoopIndexWithDelay (lookaheadSeconds));
			}
		}
	}

	IEnumerator UpdateLoopIndexWithDelay(float delayTime) {
		yield return new WaitForSeconds(delayTime);

		if (scheduledLoopIndex != currentLoopIndex) {
			currentLoopIndex = scheduledLoopIndex;
			MixLoopsForCurrentLoopIndex ();
		}

	}

	void Update () {
		if (Input.GetButtonDown("Fire2")) {

			scheduledLoopIndex = (scheduledLoopIndex + 1) % loops.Length;
		}
	}
}
