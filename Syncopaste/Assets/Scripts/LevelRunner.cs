using UnityEngine;
using System.Collections;
using SmfLite;
using System.Collections.Generic;

public class LevelRunner : MidiEventListener {

	private byte onBeatNote = 36;
	private byte offBeatNote = 43;

	private LevelModel level = null;
	private int beatCount = 0;

	public void SetLevel(LevelModel l) {
		level = l;
	}

	public void Reset() {
		beatCount = 0;
	}

	public override void HandleMidiEvent(MidiEvent e, float lookaheadSeconds) {
		StartCoroutine(HandleEventAfterDelay (e, lookaheadSeconds));
	}
	
	public override bool RespondsToMidiEvent(MidiEvent e) {
		return e.status == 144 && e.data1 == onBeatNote;
	}

	IEnumerator HandleEventAfterDelay(MidiEvent e, float delayTime) {
		yield return new WaitForSeconds(delayTime);

		if (level != null) {
			if (level.HasActionsForBeat(beatCount)) {
				List<ILevelAction> actions = level.GetActionsForBeat(beatCount);
				Debug.Log(actions.Count + " actions for beat " + beatCount);
				foreach (ILevelAction action in actions) {
					action.PerformAction();
				}
			} else {
				Debug.Log("No actions for beat " + beatCount);
			}
			
			beatCount++;
		}
	}
}
