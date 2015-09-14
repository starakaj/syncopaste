using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LevelActionType {
	Unknown = 0,
	Spawn
}

public interface ILevelAction {
	LevelActionType Type();
	void PerformAction ();
}

public struct SpawnLevelAction: ILevelAction {

	private int count;

	public SpawnLevelAction(int count) {
		this.count = count;
	}

	public LevelActionType Type() {
		return LevelActionType.Spawn;
	}

	public int GetCount() {
		return count;
	}

	public void PerformAction() {
			
		if (count == 0)
			return;
		
		float xoffset = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2f;
		float colWidth = (Screen.width / PixelPerfectCamera.pixelsToUnits) / count;
		float spawnY = (Screen.height / PixelPerfectCamera.pixelsToUnits) - 2;
		
		for (int i=0; i<count; ++i) {
			GameObject enemyPrefab = GameObject.Find("GameManager").GetComponent<GameManager>().enemyPrefab;
			float spawnX = colWidth * ((float)i + 0.5f) - xoffset;
			Vector3 pos = new Vector3(spawnX, spawnY, 0);
			GameObject enemy = GameObjectUtil.Instantiate(enemyPrefab, pos);
			CollidableObjectModel collidable = enemy.GetComponent<CollidableObjectModel> ();
			collidable.beatType = SongData.GetRandomBeat();
			enemy.GetComponent<SpriteRenderer>().material.color = ShipViewModel.ColorForBeatType(collidable.beatType);
		}
		
		Debug.Log ("Spawning " + count + " enemies");
	}
}

public class LevelModel {

	private Dictionary<int, ILevelAction> beatActions = new Dictionary<int, ILevelAction> ();

	public LevelModel(TextAsset jsonFile) {
		string fileString = jsonFile.text;
		JSONObject j = new JSONObject (fileString);

		Debug.Assert (j.type == JSONObject.Type.OBJECT);
		for (int i=0; i<j.list.Count; i++) {
			string key = (string)j.keys[i];
			JSONObject encodedAction = (JSONObject)j.list[i];
			Debug.Assert(encodedAction.type == JSONObject.Type.NUMBER);
			SpawnLevelAction l = new SpawnLevelAction((int)encodedAction.n);

			int beat = int.Parse(key);
			beatActions[beat] = l;
		}
	}

	public bool HasActionForBeat(int beat) {
		return beatActions.ContainsKey (beat);
	}

	public ILevelAction GetActionForBeat(int beat) {
		return beatActions [beat];
	}
}
