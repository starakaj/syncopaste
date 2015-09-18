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

	private float locationX;
	private float locationY;
	private float velocityX;
	private float velocityY;

	public SpawnLevelAction(JSONObject options) {

		locationX = 0f;
		locationY = 0f;
		velocityX = 0f;
		velocityY = 0f;

		if (options ["locationX"] != null)
			this.locationX = (float)options ["locationX"].f;
		if (options ["locationY"] != null)
			this.locationY = (float)options ["locationY"].f;
		if (options ["velocityX"] != null) 
			this.velocityX = (float)options ["velocityX"].f;
		if (options ["velocityY"] != null) 
			this.velocityY = (float)options ["velocityY"].f;
	}

	public LevelActionType Type() {
		return LevelActionType.Spawn;
	}

	public void PerformAction() {
		
		float spawnX = ((Screen.width / PixelPerfectCamera.pixelsToUnits) / 2f) * locationX;
		float spawnY = ((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2f) * locationY;

		GameObject enemyPrefab = GameObject.Find("GameManager").GetComponent<GameManager>().enemyPrefab;
		Vector3 pos = new Vector3(spawnX, spawnY, 0);
		GameObject enemy = GameObjectUtil.Instantiate(enemyPrefab, pos);
		enemy.GetComponent<InstantVelocity>().velocity = new Vector2 (velocityX, velocityY);	
		CollidableObjectModel collidable = enemy.GetComponent<CollidableObjectModel> ();
		collidable.beatType = SongData.GetRandomBeat();
		enemy.GetComponent<SpriteRenderer>().material.color = ShipViewModel.ColorForBeatType(collidable.beatType);
	}
}

public class LevelModel {

	private Dictionary<int, List<ILevelAction>> beatActions = new Dictionary<int, List<ILevelAction>> ();

	private ILevelAction LevelActionWithType(LevelActionType type, JSONObject options) {
		ILevelAction action = null;

		switch (type) {
		case LevelActionType.Spawn:
			action = new SpawnLevelAction(options);
			break;
		}

		return action;
	}

	private LevelActionType LevelActionTypeForName(string name) {
		if (name.Equals ("spawn"))
			return LevelActionType.Spawn;

		return LevelActionType.Unknown;
	}

	public LevelModel(TextAsset jsonFile) {
		string fileString = jsonFile.text;
		JSONObject j = new JSONObject (fileString);

		Debug.Assert (j.type == JSONObject.Type.OBJECT);
		for (int i=0; i<j.list.Count; i++) {

			string key = (string)j.keys[i];
			int beat = int.Parse(key);
			JSONObject encodedActions = (JSONObject)j.list[i]; // Array of actions
			Debug.Assert(encodedActions.type == JSONObject.Type.ARRAY);

			for (int k=0; k<encodedActions.list.Count; k++) {
				JSONObject encodedAction = (JSONObject)encodedActions.list[k];
				Debug.Assert(encodedAction.type == JSONObject.Type.OBJECT);

				Debug.Assert(encodedAction["type"] != null);
				JSONObject typeObject = encodedAction["type"];
				JSONObject optionsObject = encodedAction["options"];
				Debug.Assert(typeObject.type == JSONObject.Type.STRING);

				LevelActionType type = LevelActionTypeForName(typeObject.str);
				Debug.Assert(type != LevelActionType.Unknown);

				ILevelAction action = LevelActionWithType(type, optionsObject);
				Debug.Assert(action != null);


				if (!beatActions.ContainsKey(beat)) {
					beatActions[beat] = new List<ILevelAction>();
				}
				List<ILevelAction> actions = beatActions[beat];
				actions.Add(action);
			}
		}
	}

	public bool HasActionsForBeat(int beat) {
		return beatActions.ContainsKey (beat);
	}

	public List<ILevelAction> GetActionsForBeat(int beat) {
		return beatActions [beat];
	}
}
