using UnityEngine;
using System.Collections;
using SmfLite;

// Probably override this whole thing when you inherit
public abstract class MidiEventListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	abstract public void HandleMidiEvent(MidiEvent e, float lookaheadSeconds);

	abstract public bool RespondsToMidiEvent(MidiEvent e);
}
