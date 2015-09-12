using UnityEngine;
using System.Collections;
using SmfLite;

public class EventStore : MidiEventListener {

	private MidiEvent? mevent;
	private float endValidTime;
	private IEnumerator currentCoroutine;

	private float leadingCushionSeconds = 0.1f;
	private float trailingCushionSeconds = 0.2f;

	public override void HandleMidiEvent(MidiEvent e, float lookaheadSeconds) {

		currentCoroutine = AddMidiEventWithTimeBoundaries (e, lookaheadSeconds - leadingCushionSeconds, lookaheadSeconds + trailingCushionSeconds);
		StartCoroutine (currentCoroutine);
	}

	public override bool RespondsToMidiEvent(MidiEvent e) {
		return e.status == 144;
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
