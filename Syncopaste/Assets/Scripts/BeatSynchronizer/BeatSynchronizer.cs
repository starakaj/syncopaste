using UnityEngine;
using System.Collections;

/// <summary>
/// This class should be attached to the audio source for which synchronization should occur, and is 
/// responsible for synching up the beginning of the audio clip with all active beat counters and pattern counters.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BeatSynchronizer : MonoBehaviour {

	public float bpm = 120f;		// Tempo in beats per minute of the audio clip.
	public float startDelay = 1f;	// Number of seconds to delay the start of audio playback.
	public float lookAhead = 0.2f;	// Number of seconds in advance that each listener should run
	public delegate void AudioStartAction(double syncTime, double lookahead);
	public static event AudioStartAction OnAudioStart;

	public void Play () {
		Debug.Assert (startDelay > lookAhead);

		double initTime = AudioSettings.dspTime;
		GetComponent<AudioSource>().PlayScheduled(initTime + startDelay);
		if (OnAudioStart != null) {
			OnAudioStart(initTime + startDelay - lookAhead, lookAhead);
		}
	}

	public void Stop () {
		GetComponent<AudioSource> ().Stop ();
	}

}
