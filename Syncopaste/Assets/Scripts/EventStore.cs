using UnityEngine;
using System.Collections;
using SmfLite;

public class EventStore : MidiEventListener {

	public MIDICounter midiCounter;

	private MidiEvent? mevent;
	private float endValidTime;
	private IEnumerator currentCoroutine;

	private float leadingCushionSeconds = 0.1f;
	private float trailingCushionSeconds = 0.2f;

	public override void HandleMidiEvent(MidiEvent e, float lookaheadSeconds, MIDICounter source) {

		currentCoroutine = AddMidiEventWithTimeBoundaries (e, lookaheadSeconds - leadingCushionSeconds, lookaheadSeconds + trailingCushionSeconds);
		StartCoroutine (currentCoroutine);
	}

	public override bool RespondsToMidiEvent(MidiEvent e, MIDICounter source) {
		return midiCounter == source && e.status == 144;
	}

	public MidiEvent? GetCurrentMidiEvent() {

		if (mevent.HasValue) {
			MidiEvent? retEvent = mevent;
			mevent = null;
			if (Time.time < endValidTime) {
				return retEvent;
			}
		}

		return null;
	}

	IEnumerator AddMidiEventWithTimeBoundaries (MidiEvent e, float startSeconds, float endSeconds) {

		yield return new WaitForSeconds(startSeconds);
	
		mevent = e;
		endValidTime = Time.time + (endSeconds - startSeconds);
	}
}
