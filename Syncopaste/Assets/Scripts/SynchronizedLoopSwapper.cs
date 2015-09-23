using UnityEngine;
using System.Collections;

public class SynchronizedLoopSwapper : MonoBehaviour {

	public AudioSource[] sources;

	private int currentLoopIndex;
	private float cueSyncTime;

	private void StartCuePlayback(double syncTime, double lookaheadSeconds) {
		cueSyncTime = (float)(syncTime + lookaheadSeconds);
		AudioSource s = sources [currentLoopIndex];

		if (s && s.enabled) {
			s.PlayScheduled(cueSyncTime);
		}
	}

	private void StopCuePlayback() {
		AudioSource s = sources [currentLoopIndex];
		s.Stop ();
	}

	void OnEnable() {
		BeatSynchronizer.OnAudioStart += StartCuePlayback;
		BeatSynchronizer.OnAudioStop += StopCuePlayback;
	}

	public void SetActiveLoopIndex(int index) {
		Debug.Assert (index >= 0 && index < sources.Length);

		if (currentLoopIndex == index)
			return;

		AudioSource oldSource = sources [currentLoopIndex];
		AudioSource newSource = sources [index];
		currentLoopIndex = index;

		float currentDSPTime = (float) AudioSettings.dspTime;
		float loopCount = Mathf.Floor((currentDSPTime - cueSyncTime) / oldSource.clip.length);
		float swapTime = cueSyncTime + (loopCount + 1) * oldSource.clip.length;

		if (oldSource.isPlaying)
			oldSource.SetScheduledEndTime (swapTime);
		if (newSource.isPlaying) {
			newSource.SetScheduledEndTime (swapTime + 36000.0); // No one's going to play for ten hours...
		} else {
			newSource.PlayScheduled (swapTime);
		}
	}

	void Update() {
		if (Input.GetButtonDown("Fire2")) {
			int newLoopIndex = (currentLoopIndex + 1) % sources.Length;
			SetActiveLoopIndex(newLoopIndex);
		}
	}
}
