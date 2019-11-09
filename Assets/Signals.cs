﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

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
		controllerKinematics = new OVRKinematics();
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

	public OVRKinematics(string g="h")
	{
		posR = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
		velR = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
		accR = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
		rotR = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
		rotREuler = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles;
		rotdotR = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
		rotdotdotR = OVRInput.GetLocalControllerAngularAcceleration(OVRInput.Controller.RTouch);
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
}
