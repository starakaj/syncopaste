using UnityEngine;
using System.Collections;

public class SynchronizedAudioSwapper : BeatEventListener {

	public AudioClip[] loops = new AudioClip[2];
	public AudioClip[] paddedLoops = new AudioClip[2];

	private AudioSource[] sourcesA;
	private AudioSource[] sourcesB;
	private AudioSource[] activeSources;
	private AudioSource[] inactiveSources;

	private int currentLoopIndex = 0;
	private int scheduledLoopIndex = 0;
	private float cueSyncTime;

	private Coroutine silenceSourceRoutine;
	private Coroutine swapPaddedForTrimmedRoutine;

	// Use this for initialization
	void Start () {

		sourcesA = new AudioSource[3];
		sourcesA [0] = gameObject.AddComponent<AudioSource> ();
		sourcesA [0].clip = loops [0];
		sourcesA [1] = gameObject.AddComponent<AudioSource> ();
		sourcesA [1].clip = paddedLoops [0];
		sourcesA [2] = gameObject.AddComponent<AudioSource> ();
		sourcesA [2].clip = paddedLoops [0];
		sourcesA [2].time = sourcesA [0].clip.length;

		sourcesA [0].loop = true;
		sourcesA [1].loop = true;
		sourcesA [2].loop = true;
		sourcesA [1].volume = 0;
		sourcesA [2].volume = 0;

		sourcesB = new AudioSource[3];
		sourcesB [0] = gameObject.AddComponent<AudioSource> ();
		sourcesB [0].clip = loops [1];
		sourcesB [1] = gameObject.AddComponent<AudioSource> ();
		sourcesB [1].clip = paddedLoops [1];
		sourcesB [2] = gameObject.AddComponent<AudioSource> ();
		sourcesB [2].clip = paddedLoops [1];
		sourcesB [2].time = sourcesB [0].clip.length;

		sourcesB [0].loop = true;
		sourcesB [1].loop = true;
		sourcesB [2].loop = true;
		sourcesB [0].volume = 0;
		sourcesB [1].volume = 0;
		sourcesB [2].volume = 0;
	}

	void OnEnable ()
	{
		BeatSynchronizer.OnAudioStart += CueAudioSources;
		BeatSynchronizer.OnAudioStop += StopAudioSources;
	}
	
	void OnDisable ()
	{
		BeatSynchronizer.OnAudioStart -= CueAudioSources;
		BeatSynchronizer.OnAudioStop -= StopAudioSources;
	}

	private void CueAudioSources (double syncTime, double lookahead) {
		cueSyncTime = (float)syncTime;
		foreach (AudioSource s in sourcesA)
			s.PlayScheduled (syncTime + lookahead);
		foreach (AudioSource s in sourcesB)
			s.PlayScheduled (syncTime + lookahead);
	}

	private void StopAudioSources () {
		foreach (AudioSource s in sourcesA)
			s.Stop ();
		foreach (AudioSource s in sourcesB)
			s.Stop ();
	}

	private int IndexOfSilentPaddedLoops() {
		float currentTime = (float)AudioSettings.dspTime;
		int completedLoopCount = Mathf.FloorToInt ((currentTime - cueSyncTime) / sourcesA [0].clip.length);
		return ((completedLoopCount + 1) % 2) + 1;
	}

	private void SwapActiveInactive() {
		AudioSource[] tmpSources = activeSources;
		activeSources = inactiveSources;
		inactiveSources = tmpSources;
	}

	private void MixLoopsForCurrentLoopIndex (float fadeTime) {
		int silentIndex = IndexOfSilentPaddedLoops();
		int activeIndex = (silentIndex % 2) + 1;

		float currentTime = (float)AudioSettings.dspTime;
		float clipTime = sourcesA [0].clip.length;
		float timeToNextLoopMidpoint = clipTime - (currentTime % clipTime) + clipTime / 2.0f;
		silenceSourceRoutine = StartCoroutine(SilenceAudioSourceAfterDelay(activeSources[activeIndex], timeToNextLoopMidpoint));
		swapPaddedForTrimmedRoutine = StartCoroutine(SwapAudioSourcesAfterDelay(inactiveSources[silentIndex], inactiveSources[0], timeToNextLoopMidpoint));

		SwapActiveInactive ();
	}

	private IEnumerator SilenceAudioSourceAfterDelay (AudioSource source, float delayTime) {
		yield return new WaitForSeconds (delayTime);

		source.volume = 0;
	}

	private IEnumerator SwapAudioSourcesAfterDelay (AudioSource outSource, AudioSource inSource, float delayTime) {
		yield return new WaitForSeconds (delayTime);

		outSource.volume = 0;
		inSource.volume = 1;
	}

	override public void HandleBeatEvent(int beat, int beatsPerMeasure, float lookaheadSeconds) {
		if (beat == 0) {

		}
	}

//	IEnumerator UpdateLoopIndexWithDelay(float delayTime) {
//
//	}

	void Update () {
		if (Input.GetButtonDown("Fire2")) {
			scheduledLoopIndex = (scheduledLoopIndex + 1) % loops.Length;
			MixLoopsForCurrentLoopIndex (1.0f);
		}
	}
}
