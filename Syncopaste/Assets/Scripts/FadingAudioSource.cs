using UnityEngine;
using System.Threading;

/// <summary>
///   Audio source that fades between clips instead of playing them immediately.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FadingAudioSource : MonoBehaviour
{
	#region Fields	
	/// <summary>
	///   Actual audio source.
	/// </summary>
	public AudioSource audioSource;
	public delegate void OnFadeComplete(bool didFinish);
	
	/// <summary>
	///   Whether the audio source is currently fading, in or out.
	/// </summary>
	private FadeState fadeState = FadeState.None;
	private OnFadeComplete completionCallback;
	private float targetVolume;
	private float fadeDuration;
	private float progress;
	private float startTime;
	private float startVolume;
	#endregion
	
	#region Enums
	public enum FadeState
	{
		None,
		Fading
	}
	#endregion

	#region Public Methods and Operators
	public void FadeTo(float volume, float duration, OnFadeComplete completeCallback)
	{
		if (fadeState == FadeState.Fading) {
			if (completionCallback != null) {
				completionCallback(false);
			}
		}

		if (audioSource == null)
			return;

		targetVolume = volume;
		startTime = Time.time;
		startVolume = audioSource.volume;
		completionCallback = completeCallback;
		fadeState = FadeState.Fading;
		fadeDuration = duration;
	}
	#endregion
	
	#region Methods
	private void Update()
	{
		if (!this.audioSource.enabled)
		{
			return;
		}

		if (fadeState == FadeState.Fading) {
			float elapsedTime = Time.time - startTime;
			float progress = fadeDuration == 0f ? 1 : elapsedTime / fadeDuration;
			float clippedProgress = Mathf.Clamp(progress, 0f, 1f);

			audioSource.volume = (targetVolume - startVolume) * clippedProgress + startVolume;
			Debug.Log(audioSource.volume);

			if (elapsedTime >= fadeDuration) {
				fadeState = FadeState.None;
				if (completionCallback != null) {
					completionCallback(true);
				}
			}
		}
	}
	#endregion
}