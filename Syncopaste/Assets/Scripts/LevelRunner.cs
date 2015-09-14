using UnityEngine;
using System.Collections;
using SmfLite;

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
		if (level != null) {
			if (level.HasActionForBeat(beatCount)) {
				ILevelAction action = level.GetActionForBeat(beatCount);
				action.PerformAction();
			}

			beatCount++;
		}
	}
	
	public override bool RespondsToMidiEvent(MidiEvent e) {
		return e.status == 144 && e.data1 == onBeatNote;
	}
}
