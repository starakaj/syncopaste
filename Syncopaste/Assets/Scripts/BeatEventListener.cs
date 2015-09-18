using UnityEngine;
using System.Collections;

public abstract class BeatEventListener : MonoBehaviour {

	abstract public void HandleBeatEvent(int beat, int beatsPerMeasure, float lookaheadSeconds);
}
