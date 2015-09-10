using UnityEngine;
using System.Collections;
using SynchronizerData;
using SmfLite;

public class MIDICounter : MonoBehaviour {

	public float bpm = 80f;
	public float loopTime = 30f;
	public AudioSource audioSource;
	public GameObject[] observers;
	public TextAsset midiFile;

	private MidiTrackSequencer midiSeq;
	private float timeOffset;
	private float lastAdvanceTime;

	/// <summary>
	/// Initializes and starts the coroutine that checks for beat occurrences in the pattern. The nextBeatSample field is initialized to 
	/// exactly match up with the sample that corresponds to the time the audioSource clip started playing (via PlayScheduled).
	/// </summary>
	/// <param name="syncTime">Equal to the audio system's dsp time plus the specified delay time.</param>
	void StartAdvancePattern (double syncTime)
	{
		timeOffset = (float)syncTime;
		MidiFileContainer fileContainer = MidiFileLoader.Load (midiFile.bytes);
		midiSeq = new MidiTrackSequencer (fileContainer.tracks [0], fileContainer.division, bpm);

		StartCoroutine(AdvancePattern());
	}
	
	/// <summary>
	/// Subscribe the PatternCheck() coroutine to the beat synchronizer's event.
	/// </summary>
	void OnEnable ()
	{
		BeatSynchronizer.OnAudioStart += StartAdvancePattern;
	}
	
	/// <summary>
	/// Unsubscribe the PatternCheck() coroutine from the beat synchronizer's event.
	/// </summary>
	/// <remarks>
	/// This should NOT (and does not) call StopCoroutine. It simply removes the function that was added to the
	/// event delegate in OnEnable().
	/// </remarks>
	void OnDisable ()
	{
		BeatSynchronizer.OnAudioStart -= StartAdvancePattern;
	}

	IEnumerator AdvancePattern ()
	{
		while (audioSource.isPlaying) {
			var currentTime = (float)AudioSettings.dspTime;
			Debug.Log ("DSP time: " + currentTime);

			if (currentTime > timeOffset) {

				if (!midiSeq.Playing) {
					foreach (MidiEvent e in midiSeq.Start ()) {
						HandleMidiEvent(e);
					}
					lastAdvanceTime = 0f;
				} else {
					var deltaDSPTime = currentTime - lastAdvanceTime - timeOffset;
					foreach (SmfLite.MidiEvent e in midiSeq.Advance (deltaDSPTime)) {
						HandleMidiEvent (e);
					}
					lastAdvanceTime = currentTime - timeOffset;
				}

			}
			
			yield return new WaitForSeconds(loopTime / 1000f);
		}
	}

	void HandleMidiEvent (MidiEvent e) {
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
