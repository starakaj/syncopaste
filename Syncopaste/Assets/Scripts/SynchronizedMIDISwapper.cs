using UnityEngine;
using System.Collections;

public class SynchronizedMIDISwapper : BeatEventListener {

	public MIDICounter[] midiLoops;

	private int currentLoopIndex = 0;
	private int scheduledLoopIndex = 0;

	// Use this for initialization
	void Start () {
		MixLoopsForCurrentLoopIndex ();
	}

	public MIDICounter ActiveMIDICounter() {
		return midiLoops [currentLoopIndex];
	}
	
	private void MixLoopsForCurrentLoopIndex () {
		for (int i=0; i<midiLoops.Length; ++i) {
			midiLoops[i].outputEnabled = true; // No-op for now, I suppose
		}
	}

	public int GetActiveLoopIndex() {
		return currentLoopIndex;
	}
	
	override public void HandleBeatEvent(int beat, int beatsPerMeasure, float lookaheadSeconds) {
		if ((beat % beatsPerMeasure) == 0) {
			if (scheduledLoopIndex != currentLoopIndex) {
				StartCoroutine (UpdateLoopIndexWithDelay (lookaheadSeconds));
			}
		}
	}
	
	IEnumerator UpdateLoopIndexWithDelay(float delayTime) {
		yield return new WaitForSeconds(delayTime - 0.1f);
		
		if (scheduledLoopIndex != currentLoopIndex) {
			currentLoopIndex = scheduledLoopIndex;
			MixLoopsForCurrentLoopIndex ();
		}
		
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2")) {
			
			scheduledLoopIndex = (scheduledLoopIndex + 1) % midiLoops.Length;
		}
	}
}
