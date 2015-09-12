using UnityEngine;
using System.Collections;
using SmfLite;

public class UpdateShipAppearance: MidiEventListener {

	private byte onBeatNote = 36;
	private byte offBeatNote = 43;

	public override void HandleMidiEvent(MidiEvent e, float lookaheadSeconds) {
		StartCoroutine(UpdateColorForMidiEventWithDelay (e, lookaheadSeconds));
	}

	public override bool RespondsToMidiEvent(MidiEvent e) {
		return e.status == 144;
	}
	
	IEnumerator UpdateColorForMidiEventWithDelay(MidiEvent e, float lookahead) {

		yield return new WaitForSeconds (lookahead);

		SongData.BeatType beatType = SongData.BeatType.None;
		if (e.data1 == onBeatNote) 
			beatType = SongData.BeatType.OnBeat;
		else if (e.data1 == offBeatNote)
			beatType = SongData.BeatType.OffBeat;

		Color c = ShipViewModel.ColorForBeatType (beatType);

		gameObject.GetComponent<SpriteRenderer> ().material.color = c;
	}
}
