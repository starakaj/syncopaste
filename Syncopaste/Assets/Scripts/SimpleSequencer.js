#pragma strict

private var source: AudioSource;
public var kick: AudioClip;
public var snare: AudioClip;
public var hat: AudioClip;
public var isPlaying: boolean = true;
public var bpm: float = 120;

private var stepCount = 16;
public var kickSequence:boolean[] = new boolean[stepCount];
public var snareSequence:boolean[] = new boolean[stepCount];
public var hatSequence:boolean[] = new boolean[stepCount];

private var step = 0;

function Awake () {
	source = GetComponent("AudioSource") as AudioSource;
}

function Start () {
	StartCoroutine(Step());
}

function TriggerSound(which) {
	var triggerListeners = FindObjectsOfType(TriggerListener) as TriggerListener[];
	for (var tl : TriggerListener in triggerListeners) {
		if (tl.type == which) {
			tl.Trigger();
		}
	}
	
	if (which == 0) {
		source.PlayOneShot(kick, 0.5);
	}
	
	if (which == 1) {
		source.PlayOneShot(snare, 0.5);
	}
	
	if (which == 2) {
		source.PlayOneShot(hat, 0.5);
	}
}

function Step (): IEnumerator {
	Debug.Log("Step: " + step);
	if (kickSequence[step]) {
		TriggerSound(0);
	}
	
	if (snareSequence[step]) {
		TriggerSound(1);
	}
	
	if (hatSequence[step]) {
		TriggerSound(2);
	}
	
	yield WaitForSeconds(60 / (bpm * 4));
	step = (step + 1) % stepCount;
	
	StartCoroutine(Step());
}