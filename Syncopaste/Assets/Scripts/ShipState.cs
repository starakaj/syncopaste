using UnityEngine;
using System.Collections;

public class ShipState : MonoBehaviour {

	public ShipData.ShipBeatType beatType;

	public Color ColorForBeatType(ShipData.ShipBeatType beatType) {

		Color retColor;

		switch (beatType) {
		case ShipData.ShipBeatType.OnBeat:
			retColor = Color.green;
			break;
		default:
		case ShipData.ShipBeatType.None:
			retColor = Color.gray;
			break;
		}

		return retColor;
	}
}
