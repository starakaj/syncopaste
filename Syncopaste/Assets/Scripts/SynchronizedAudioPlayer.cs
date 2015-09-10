using UnityEngine;
using System.Collections;

public class SynchronizedAudioPlayer : MonoBehaviour {

	public TextAsset midiFile;
	public AudioClip audioFile;
	public float bpm = 80f;
	
	private SmfLite.MidiTrackSequencer midiSeq;
	private AudioSource source;
	private float secondsPerSample;
	private float lastPlayTime;

	private float cumulativeFrameTime;
	private float cumulativeSongTime;

	public void Awake () {
		source = GetComponent<AudioSource> ();
		secondsPerSample = 1f / audioFile.frequency;
	}

	public void Play() {
		SmfLite.MidiFileContainer song = SmfLite.MidiFileLoader.Load (midiFile.bytes);
		midiSeq = new SmfLite.MidiTrackSequencer (song.tracks [0], song.division, bpm);

		// How can one actually guarantee that these are in sync?
		lastPlayTime = 0f;
		source.loop = true;
		source.clip = audioFile;
		source.Play (0);
		cumulativeFrameTime = 0f;
		cumulativeSongTime = 0f;
		foreach (SmfLite.MidiEvent e in midiSeq.Start ()) {
			HandleMidiEvent(e);
		}
	}

	public void Stop() {
		source.Stop ();
		midiSeq = null;
	}
	
	// Update is called once per frame
	void Update () {

		if (source.isPlaying) {
			float playTime = source.timeSamples * secondsPerSample;

			if (midiSeq != null) {

				cumulativeFrameTime += Time.deltaTime;
				cumulativeSongTime += playTime - lastPlayTime;

				Debug.Log ("Frequency: " + audioFile.frequency);
				Debug.Log ("FrameTime: " + Time.deltaTime);
				Debug.Log ("SongTime: " + (playTime - lastPlayTime));
				Debug.Log ("Total Frame Time: " + cumulativeFrameTime);
				Debug.Log ("Total Song Time: " + cumulativeSongTime);
				Debug.Log ("Difference: " + (cumulativeSongTime - cumulativeFrameTime));

				foreach (SmfLite.MidiEvent e in midiSeq.Advance (playTime - lastPlayTime)) {
					HandleMidiEvent (e);
				}
			}

			lastPlayTime = playTime;
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
