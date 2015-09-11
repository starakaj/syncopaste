using UnityEngine;
using System.Collections;

public class UpdateShipState : MidiEventListener {

	public float timingWindow = 0.1f;

	private ShipState shipState;

	void Awake () {
		shipState = GetComponent<ShipState> ();
	}

	public override void HandleMidiEvent(SmfLite.MidiEvent e, float lookaheadSeconds) {
		StartCoroutine(OpenWindow (lookaheadSeconds));
	}
	
	IEnumerator OpenWindow(float lookahead) {

		yield return new WaitForSeconds (lookahead - timingWindow/2f);

		shipState.beatType = ShipData.ShipBeatType.OnBeat;

		yield return new WaitForSeconds (timingWindow);

		shipState.beatType = ShipData.ShipBeatType.None;
	}
}
