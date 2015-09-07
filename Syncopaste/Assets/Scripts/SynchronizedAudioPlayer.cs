using UnityEngine;
using System.Collections;

public class SynchronizedAudioPlayer : MonoBehaviour {

	public TextAsset midiFile;
	public AudioClip audioFile;
	public float bpm = 140f;
	
	private SmfLite.MidiTrackSequencer midiSeq;

	// Use this for initialization
	void Start () {
		AudioSource source = GetComponent<AudioSource> () as AudioSource;
		SmfLite.MidiFileContainer song = SmfLite.MidiFileLoader.Load (midiFile.bytes);
		midiSeq = new SmfLite.MidiTrackSequencer (song.tracks [0], song.division, bpm);

		// How can one actually guarantee that these are in sync?
		source.loop = true;
		source.clip = audioFile;
		source.Play (0);
		foreach (SmfLite.MidiEvent e in midiSeq.Start ()) {
			HandleMidiEvent(e);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (midiSeq.Playing) {
			foreach (SmfLite.MidiEvent e in midiSeq.Advance (Time.deltaTime)) {
				HandleMidiEvent (e);
			}
		}
	}

	void HandleMidiEvent (SmfLite.MidiEvent e) {
		MidiEventListener[] listeners = FindObjectsOfType<MidiEventListener> ();
		Debug.Log ("Event: " + e);
		foreach (MidiEventListener l in listeners) {
			if (e.status == 144 || !l.ignoreNoteOff) {
				if (l.note == e.data1) {
					l.HandleMidiEvent(e);
				}
			}
		}
	}
}
