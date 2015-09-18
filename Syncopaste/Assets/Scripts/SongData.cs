using System;
using System.Collections;
using System.Collections.Generic;

public class SongData {

	public enum BeatType : int {
		None			= 0,
		OnBeat			= 1,
		OffBeat			= 2,
		SyncoBeat		= 3,
		BEAT_TYPE_LEN
	};

	public static SongData.BeatType GetRandomBeat() {
		Array values = Enum.GetValues(typeof(SongData.BeatType));
		return (SongData.BeatType)values.GetValue (UnityEngine.Random.Range ((int)SongData.BeatType.OnBeat, (int)SongData.BeatType.BEAT_TYPE_LEN));
	}
}