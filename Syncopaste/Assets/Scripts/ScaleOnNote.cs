using UnityEngine;
using System.Collections;
using SmfLite;

public class ScaleOnNote : MidiEventListener {

	public byte note;
	public Vector2 scaleRange = new Vector2 (5f, 25f);

	public override void HandleMidiEvent(MidiEvent e, float lookaheadSeconds, MIDICounter source) {
		StartCoroutine(PulseAfterDelay (lookaheadSeconds));
	}

	public override bool RespondsToMidiEvent(MidiEvent e, MIDICounter source) {
		return (e.status == 144 && e.data2 == note);
	}

	IEnumerator PulseAfterDelay(float delay) {
		yield return new WaitForSeconds (delay);

		float scaleAmt = Random.Range (scaleRange.x, scaleRange.y);
		iTween.ScaleTo(gameObject, iTween.Hash("x", scaleAmt, "y", scaleAmt, "z", 1, "easeType", "easeOutQuad", "time", 0.25, "onStart", "resetScale"));
	}

	void resetScale() {
		transform.localScale = new Vector3 (5, 5, 1);
	}
}
