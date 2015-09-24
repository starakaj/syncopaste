using UnityEngine;
using System.Collections;
using SmfLite;

public class UpdateShipAppearance: MidiEventListener {

	private byte onBeatNote = 36;
	private byte offBeatNote = 43;
	private byte syncoBeatNote = 44;

	public override void HandleMidiEvent(MidiEvent e, float lookaheadSeconds, MIDICounter source) {
		StartCoroutine(UpdateColorForMidiEventWithDelay (e, lookaheadSeconds, source));
	}

	public override bool RespondsToMidiEvent(MidiEvent e, MIDICounter source) {
		return e.status == 144;
	}
	
	IEnumerator UpdateColorForMidiEventWithDelay(MidiEvent e, float lookahead, MIDICounter source) {

		yield return new WaitForSeconds (lookahead);

		SynchronizedMIDISwapper swapper = GameObject.Find ("MidiManager").GetComponent<SynchronizedMIDISwapper> ();

		if (swapper.ActiveMIDICounter () == source) {
			SongData.BeatType beatType = SongData.BeatType.None;
			if (e.data1 == onBeatNote) 
				beatType = SongData.BeatType.OnBeat;
			else if (e.data1 == offBeatNote)
				beatType = SongData.BeatType.OffBeat;
			else if (e.data1 == syncoBeatNote) 
				beatType = SongData.BeatType.SyncoBeat;

			Color c = ShipViewModel.ColorForBeatType (beatType);

			gameObject.GetComponent<SpriteRenderer> ().material.color = c;
		} 
	}
}
