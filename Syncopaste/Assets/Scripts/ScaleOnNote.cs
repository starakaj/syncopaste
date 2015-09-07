using UnityEngine;
using System.Collections;

public class ScaleOnNote : MidiEventListener {

	public Vector2 scaleRange = new Vector2 (5f, 25f);

	public override void HandleMidiEvent(SmfLite.MidiEvent e) {
		float scaleAmt = Random.Range (scaleRange.x, scaleRange.y);
//		transform.localScale = new Vector3 (scaleAmt, scaleAmt, 1);
		iTween.ScaleTo(gameObject, iTween.Hash("x", scaleAmt, "y", scaleAmt, "z", 1, "easeType", "easeOutQuad", "time", 0.25, "onStart", "resetScale"));
	}

	void resetScale() {
		transform.localScale = new Vector3 (5, 5, 1);
	}
}
