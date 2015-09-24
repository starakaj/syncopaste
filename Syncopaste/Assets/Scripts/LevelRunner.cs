using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRunner : BeatEventListener {

	private LevelModel level = null;

	public void SetLevel(LevelModel l) {
		level = l;
	}

	public void Reset() {
	}

	public override void HandleBeatEvent(int beat, int beatsPerMeasure, float lookaheadSeconds) {
		StartCoroutine (HandleBeatAfterDelay (beat, beatsPerMeasure, lookaheadSeconds));
	}

	IEnumerator HandleBeatAfterDelay(int beat, int beatsPerMeasure, float delayTime) {
		yield return new WaitForSeconds(delayTime);

		int modBeat = beat % beatsPerMeasure;
		int measure = beat / beatsPerMeasure;

		if (level != null) {

			if (level.HasActionsForBeat(modBeat, measure)) {
				Debug.Log("Running actions for beat " + modBeat + " measure " + measure);
				List<ILevelAction> actions = level.GetActionsForBeat(modBeat, measure);
				foreach (ILevelAction action in actions) {
					action.PerformAction();
				}
			}
		}
	}
}
