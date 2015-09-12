using UnityEngine;
using System.Collections;

public class ShipViewModel {

	public static Color ColorForBeatType(SongData.BeatType beatType) {

		Color retColor;

		switch (beatType) {
		case SongData.BeatType.OnBeat:
			retColor = Color.green;
			break;
		case SongData.BeatType.OffBeat:
			retColor = Color.red;
			break;
		case SongData.BeatType.None:
		default:
			retColor = Color.gray;
			break;
		}

		return retColor;
	}
}
