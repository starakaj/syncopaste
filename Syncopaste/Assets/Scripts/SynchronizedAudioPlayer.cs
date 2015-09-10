using UnityEngine;
using System.Collections;

public class SynchronizedAudioPlayer : MonoBehaviour {

	public TextAsset midiFile;
	public AudioClip audioFile;
	public float bpm = 140f;
	
	private SmfLite.MidiTrackSequencer midiSeq;

	public void Play() {
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

	public void Stop() {
		AudioSource source = GetComponent<AudioSource> () as AudioSource;

		source.Stop ();
		midiSeq = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (midiSeq != null && midiSeq.Playing) {
			foreach (SmfLite.MidiEvent e in midiSeq.Advance (Time.deltaTime)) {
				HandleMidiEvent (e);
			}
		}
	}

	void HandleMidiEvent (SmfLite.MidiEvent e) {
		MidiEventListener[] listeners = FindObjectsOfType<MidiEventListener> ();
		foreach (MidiEventListener l in listeners) {
			if (e.status == 144 || !l.ignoreNoteOff) {
				if (l.note == e.data1) {
					l.HandleMidiEvent(e);
				}
			}
		}
	}
}
