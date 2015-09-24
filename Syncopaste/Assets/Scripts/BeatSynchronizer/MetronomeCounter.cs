using UnityEngine;
using System.Collections;

public class MetronomeCounter : MonoBehaviour {

	public float bpm = 80f;
	public float loopTime = 30f;
	public AudioSource audioSource;
	public int beatsPerMeasure = 8;

	private float timeOffset;
	private float timeToNextBeat;
	private float lookaheadSeconds;
	private float lastAdvanceTime;
	private bool hasSentFirstBeat;
	private int nextBeat = 0;

	void StartAdvanceCounter (double syncTime, double lookahead)
	{
		lookaheadSeconds = (float)lookahead;
		
		timeOffset = (float)syncTime;
		timeToNextBeat = 0f;
		nextBeat = 0;
		hasSentFirstBeat = false;
		
		StartCoroutine(AdvanceCounter());
	}

	/// <summary>
	/// Subscribe the AdvancePattern() coroutine to the beat synchronizer's event.
	/// </summary>
	void OnEnable ()
	{
		BeatSynchronizer.OnAudioStart += StartAdvanceCounter;
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
		BeatSynchronizer.OnAudioStart -= StartAdvanceCounter;
	}

	IEnumerator AdvanceCounter ()
	{
		while (audioSource.isPlaying) {
			var currentTime = (float)AudioSettings.dspTime;
			
			if (currentTime > timeOffset) {
				
				if (!hasSentFirstBeat) {

					HandleBeatEvent(); 

					timeToNextBeat = (240f / (bpm * beatsPerMeasure)) - (currentTime - timeOffset);
					hasSentFirstBeat = true;
				} else {

					timeToNextBeat -= (currentTime - lastAdvanceTime);
					if (timeToNextBeat <= 0f) {

						HandleBeatEvent();
						timeToNextBeat = (240f / (bpm * beatsPerMeasure)) + timeToNextBeat;
					}
				}

				lastAdvanceTime = currentTime;
			}
			
			yield return new WaitForSeconds(loopTime / 1000f);
		}
	}

	void HandleBeatEvent () {
		BeatEventListener[] listeners = FindObjectsOfType<BeatEventListener> ();
		foreach (BeatEventListener l in listeners) {
			l.HandleBeatEvent(nextBeat, beatsPerMeasure, lookaheadSeconds);
		}
		nextBeat = (nextBeat + 1);
	}
}
