using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRunner : BeatEventListener {

	private LevelModel level = null;
	private int beatCount = 0;

	public void SetLevel(LevelModel l) {
		level = l;
	}

	public void Reset() {
		beatCount = 0;
	}

	public override void HandleBeatEvent(int beat, int beatsPerMeasure, float lookaheadSeconds) {
		StartCoroutine (HandleBeatAfterDelay (beat, lookaheadSeconds));
	}

	IEnumerator HandleBeatAfterDelay(int beat, float delayTime) {
		yield return new WaitForSeconds(delayTime);

		if (level != null) {
			if (level.HasActionsForBeat(beatCount)) {
				List<ILevelAction> actions = level.GetActionsForBeat(beatCount);
				foreach (ILevelAction action in actions) {
					action.PerformAction();
				}
			}
			
			beatCount++;
		}
	}
}
