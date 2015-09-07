using UnityEngine;
using System.Collections;

// Probably override this whole thing when you inherit
public abstract class MidiEventListener : MonoBehaviour {

	public bool ignoreNoteOff;
	public byte note;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	abstract public void HandleMidiEvent(SmfLite.MidiEvent e);
}
