using UnityEngine;
using System.Collections;

public class AudioSourceSynchronizer : MonoBehaviour {

	public AudioSource slaveSource;

	void OnEnable ()
	{
		BeatSynchronizer.OnAudioStart += CueAudioSource;
		BeatSynchronizer.OnAudioStop += StopAudioSource;
	}

	void OnDisable ()
	{
		BeatSynchronizer.OnAudioStart -= CueAudioSource;
		BeatSynchronizer.OnAudioStop -= StopAudioSource;
	}

	void CueAudioSource (double syncTime, double lookahead) {
		slaveSource.PlayScheduled (syncTime + lookahead); // Ignore lookahead so audio is synchronized
	}

	void StopAudioSource () {
		slaveSource.Stop ();
	}
}
