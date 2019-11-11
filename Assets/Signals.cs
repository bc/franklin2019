using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public struct Observation
{
	public float time_ms;
	public OVRKinematics controllerKinematics;
	public string reachState;
	public float perturbationProgress;
	public float frame;

	public Observation(RepresentationController rc)
	{
		time_ms = Time.time;
		frame = Time.frameCount;
		controllerKinematics = new OVRKinematics(OVRInput.Controller.RTouch);
		reachState = rc.reachingState.ToString();
		perturbationProgress = rc.perturbProgress;
	}
}

public struct OVRKinematics
{
	public Vector3 posR; //meters
	public Vector3 velR; //meters per second
	public Vector3 accR; //meters per second squared
	public Quaternion rotR; //orientation
	public Vector3 rotREuler;
	public Vector3 rotdotR; //radians per second, per dimension
	public Vector3 rotdotdotR; // radians per second per second, per dimension

	public OVRKinematics(OVRInput.Controller controller)
	{
		posR = OVRInput.GetLocalControllerPosition(controller);
		velR = OVRInput.GetLocalControllerVelocity(controller);
		accR = OVRInput.GetLocalControllerAcceleration(controller);
		rotR = OVRInput.GetLocalControllerRotation(controller);
		rotREuler = OVRInput.GetLocalControllerRotation(controller).eulerAngles;
		rotdotR = OVRInput.GetLocalControllerAngularVelocity(controller);
		rotdotdotR = OVRInput.GetLocalControllerAngularAcceleration(controller);
	}
}

[Serializable]
public struct TrialResponse
{
		public int trialIndex;
		public Participant participant;
		public Condition currentCondition;
		public Vector3 startingPosition;
		public Vector3 endingPosition;
		public Vector3 perturbationStartPosition;
		public List<Observation> observationsOverTime;
        public TrialResponse(Participant info, RepresentationController rc, Vector3 inputStart, Vector3 inputEnd, Vector3 inputPerturbStart)
        {
	        observationsOverTime = new List<Observation>(); //instantiate empty list
	        participant = info;
	        currentCondition = rc.currentCondition;
	        trialIndex = rc.trialIndex;
	        startingPosition = inputStart;
	        
	        endingPosition = inputEnd;
	        perturbationStartPosition = inputPerturbStart;
        }

        public void Observe(RepresentationController rc)
        {
	        observationsOverTime.Add(new Observation(rc));
        }
        
        internal string ToJSON()
        {
	        return JsonUtility.ToJson(this);
        }
}

public struct Participant
{
	public string notes;
	public string experimentBeginTime;
	public string uid;
	public Participant(string notes = "")
	{
		this.notes = notes;
		experimentBeginTime = DateTime.Now.ToLongTimeString();
		uid = Guid.NewGuid().ToString();
	}
}

public class Signals : MonoBehaviour
{
	public RepresentationController rc;
	public Participant participant;
	public TrialResponse obs;
	public Transform start, end, perturb;


	private void Start()
	{
		rc = GameObject.Find("HandAvatarRight").GetComponent<RepresentationController>();
		participant = new Participant("");
	}

	private void LateUpdate()
	{
		obs.Observe(rc);
	}

	internal void SaveTrialResponse()
	{
		var serializedTrialObs = obs.ToJSON();
		var filePath = Path.Combine(Application.persistentDataPath,
			$"{participant.uid}_{obs.trialIndex}.json");
		File.WriteAllText(filePath, serializedTrialObs);
		Debug.Log($" look within {filePath}");

}

	public void StartNewTrialResponse()
	{
		obs = new TrialResponse(participant, rc, start.position, end.position, perturb.position);
	}

	public AnimationCurve ReachVelocityProfile()
	{
		var curveA = new AnimationCurve();
		foreach (var v in obs.observationsOverTime)
		{
			curveA.AddKey(v.time_ms - obs.observationsOverTime[0].time_ms, v.controllerKinematics.velR.x);
		}

		return curveA;
	}

	public static OVRKinematics GetKinematics(OVRInput.Controller controller)
	{
		return new OVRKinematics(controller);
	}
}
