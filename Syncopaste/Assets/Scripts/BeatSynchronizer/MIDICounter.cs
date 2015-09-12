using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	private float lookaheadSeconds;
	
	void StartAdvancePattern (double syncTime, double lookahead)
	{
		lookaheadSeconds = (float)lookahead;

		timeOffset = (float)syncTime;
		MidiFileContainer fileContainer = MidiFileLoader.Load (midiFile.bytes);
		midiSeq = new MidiTrackSequencer (fileContainer.tracks [0], fileContainer.division, bpm);

		StartCoroutine(AdvancePattern());
	}
	
	/// <summary>
	/// Subscribe the AdvancePattern() coroutine to the beat synchronizer's event.
	/// </summary>
	void OnEnable ()
	{
		BeatSynchronizer.OnAudioStart += StartAdvancePattern;
	}
	
	/// <summary>
	/// Unsubscribe the AdvancePattern() coroutine from the beat synchronizer's event.
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

			if (currentTime > timeOffset) {

				if (!midiSeq.Playing) {
					List<MidiEvent> messages = midiSeq.Start ();
					if (messages != null) {
						foreach (MidiEvent e in messages) {
							HandleMidiEvent(e);
						}
					}
					lastAdvanceTime = 0f;
				} else {
					var deltaDSPTime = currentTime - lastAdvanceTime - timeOffset;
					List<MidiEvent> messages = midiSeq.Advance (deltaDSPTime);
					if (messages != null) {
						foreach (MidiEvent e in messages) {
							HandleMidiEvent(e);
						}
					}
					lastAdvanceTime = currentTime - timeOffset;

					// Handle looping
					if (!midiSeq.Playing) {
						timeOffset += audioSource.clip.length;
					}
				}

			}
			
			yield return new WaitForSeconds(loopTime / 1000f);
		}
	}

	void HandleMidiEvent (MidiEvent e) {
		MidiEventListener[] listeners = FindObjectsOfType<MidiEventListener> ();
		foreach (MidiEventListener l in listeners) {
			if (l.RespondsToMidiEvent(e))
				l.HandleMidiEvent(e, lookaheadSeconds);
		}
	}
}
