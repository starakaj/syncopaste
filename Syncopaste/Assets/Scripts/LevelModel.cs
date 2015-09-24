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

public struct LevelActionUtils {
	public static float FloatFromConstantOrRangeAtKey(string key, JSONObject options) {
		JSONObject o = options [key];
		if (o == null) {
			return 0f;
		} else {
			if (o.type == JSONObject.Type.NUMBER)
				return o.f;
			else if (o.type == JSONObject.Type.ARRAY) {
				if (o.Count == 2)
					return Random.Range(o[0].f, o[1].f);
			}
		}

		return 0f;
	}

	public static int IntFromConstantOrRangeAtKey(string key, JSONObject options) {
		JSONObject o = options [key];
		if (o == null) {
			return 0;
		} else {
			if (o.type == JSONObject.Type.NUMBER)
				return (int) o.i;
			else if (o.type == JSONObject.Type.ARRAY) {
				if (o.Count == 2)
					return (int) Random.Range(o[0].i, o[1].i);
			}
		}
		
		return 0;
	}
}

public struct SpawnLevelAction: ILevelAction {

	private JSONObject m_options;

	public SpawnLevelAction(JSONObject options) {
		m_options = options;
	}

	public LevelActionType Type() {
		return LevelActionType.Spawn;
	}

	public void PerformAction() {

		int count = LevelActionUtils.IntFromConstantOrRangeAtKey ("count", m_options);
		count = Mathf.Max (count, 1);

		for (int i=0; i<count; ++i) {

			float locationX = LevelActionUtils.FloatFromConstantOrRangeAtKey ("locationX", m_options);
			float locationY = LevelActionUtils.FloatFromConstantOrRangeAtKey ("locationY", m_options);
			float velocityX = LevelActionUtils.FloatFromConstantOrRangeAtKey ("velocityX", m_options);
			float velocityY = LevelActionUtils.FloatFromConstantOrRangeAtKey ("velocityY", m_options);

			float spawnX = ((Screen.width / PixelPerfectCamera.pixelsToUnits) / 2f) * locationX;
			float spawnY = ((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2f) * locationY;

			GameObject enemyPrefab = GameObject.Find ("GameManager").GetComponent<GameManager> ().enemyPrefab;
			Vector3 pos = new Vector3 (spawnX, spawnY, -1);
			GameObject enemy = GameObjectUtil.Instantiate (enemyPrefab, pos);
			enemy.GetComponent<InstantVelocity> ().velocity = new Vector2 (velocityX, velocityY);
		}
	}
}

public class LevelModel {

	private Dictionary<string, List<ILevelAction>> beatActions = new Dictionary<string, List<ILevelAction>> ();

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

	private string StringKeyForBeatAndMeasure(int beat, int measure) {
		return measure + "." + beat;
	}

	public LevelModel(TextAsset jsonFile) {
		string fileString = jsonFile.text;

		JSONObject root = new JSONObject (fileString);
		Debug.Assert (root.type == JSONObject.Type.OBJECT);

		JSONObject j = root ["beats"];

		Debug.Assert (j.type == JSONObject.Type.OBJECT);
		for (int i=0; i<j.list.Count; i++) {

			string key = (string)j.keys[i];
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


				if (!beatActions.ContainsKey(key)) {
					beatActions[key] = new List<ILevelAction>();
				}
				List<ILevelAction> actions = beatActions[key];
				actions.Add(action);
			}
		}
	}

	public bool HasActionsForBeat(int beat, int measure) {
		return beatActions.ContainsKey (StringKeyForBeatAndMeasure(beat, measure));
	}

	public List<ILevelAction> GetActionsForBeat(int beat, int measure) {
		return beatActions [StringKeyForBeatAndMeasure(beat, measure)];
	}
}
