using UnityEngine;
using System.Collections;
using SmfLite;

// Probably override this whole thing when you inherit
public abstract class MidiEventListener : MonoBehaviour {

	abstract public void HandleMidiEvent(MidiEvent e, float lookaheadSeconds, MIDICounter source);

	abstract public bool RespondsToMidiEvent(MidiEvent e, MIDICounter source);
}
