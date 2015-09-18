using UnityEngine;
using System.Collections;
using SmfLite;

public class ProjectileShooter : MonoBehaviour {
	
	public GameObject projectilePrefab;

	private byte onBeatNote = 36;
	private byte offBeatNote = 43;
	private byte syncoBeatNote = 44;

	void Update () {
		if (Input.GetButtonDown("Fire1")) {

			MidiEvent? e = GameObject.Find("GameManager").GetComponent<EventStore>().GetCurrentMidiEvent();

			if (e.HasValue) {
				SongData.BeatType beatType = SongData.BeatType.None;
				if (e.Value.data1 == onBeatNote)
					beatType = SongData.BeatType.OnBeat;
				else if (e.Value.data1 == offBeatNote) 
					beatType = SongData.BeatType.OffBeat;
				else if (e.Value.data1 == syncoBeatNote) 
					beatType = SongData.BeatType.SyncoBeat;

				GameObject projectile = GameObjectUtil.Instantiate(projectilePrefab, gameObject.transform.position);
				projectile.GetComponent<CollidableObjectModel>().beatType = beatType;
				Color c = ShipViewModel.ColorForBeatType(beatType);
				projectile.GetComponent<SpriteRenderer>().material.color = c;
			}
		}
	}
}
