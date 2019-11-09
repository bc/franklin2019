using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StoredTransform
{
	public Vector3 position;
	public Quaternion rotation;

	public StoredTransform(Vector3 myPosition, Quaternion myRotation)
	{
		position = myPosition;
		rotation = myRotation;
	}
}

public class Signals : MonoBehaviour
{
	private GameObject HMD;
	private GameObject leftController;
	private GameObject rightController;
	public GameObject leftAvatar;
	public GameObject rightAvatar;
	private List<StoredTransform> HMD_Observations = new List<StoredTransform>();
	private List<StoredTransform> leftControllerObservations = new List<StoredTransform>();
	private List<StoredTransform> rightControllerObservations = new List<StoredTransform>();
	private List<StoredTransform> leftAvatarObservations = new List<StoredTransform>();
	private List<StoredTransform> rightAvatarObservations = new List<StoredTransform>();
	private List<float> times = new List<float>();
	public List<string> currentTask = new List<string>();
	public string participantId = "null_participant_id";


	void Start() {
		HMD = GameObject.Find("CenterEyeAnchor");
		leftController = GameObject.Find("LeftControllerAnchor");
		rightController = GameObject.Find("RightControllerAnchor");
		
		GameObject taskContainerObject = GameObject.FindGameObjectsWithTag("taskContainerTag")[0];
		Debug.Log($"the leftavatar is {leftAvatar.name} and right is {rightAvatar.name}");
	}
	
	// Update is called once per frame
	void LateUpdate()
	{

		/*HMD_Observations.Add(new StoredTransform(HMD.transform.position, HMD.transform.rotation));
		leftControllerObservations.Add(new StoredTransform(leftController.transform.position, leftController.transform.rotation));
		rightControllerObservations.Add(new StoredTransform(rightController.transform.position, rightController.transform.rotation));
		leftAvatarObservations.Add(new StoredTransform(leftAvatar.transform.position, leftAvatar.transform.rotation));
		rightAvatarObservations.Add(new StoredTransform(rightAvatar.transform.position, rightAvatar.transform.rotation));
		times.Add(Time.time * 1000.0f);*/
	}

	private string RightOrLeftToString(string avatarName)
	{
		if (avatarName == "Controller (right)avatar")
		{
			return "right";
		}
		else if(avatarName == "Controller (left)avatar")
		{
			return "left";
		}
		else
		{
			Debug.LogError("You tried to record the hand but it was not a controller avatar. see Signals.cs");
			return "err";
		}
	}

	public void StoreTrialData(int block, int trial, string hand)
	{
		float milliseconds = Time.time * 1000;
		string outputFileName =
			String.Format(
				"participantId_{0}_time_{1}_block_{2}_trial{3}_hand_{4}.csv",
				participantId, milliseconds.ToString("F4"), block, trial, RightOrLeftToString(hand));
		string outputFilePath = Application.persistentDataPath + "/" + outputFileName;
		Debug.Log(outputFilePath);
		SaveSignals(outputFilePath);
		ClearOldValues();
	}
	public void ClearOldValues()
	{
		HMD_Observations = new List<StoredTransform>();
		leftControllerObservations = new List<StoredTransform>();
		rightControllerObservations = new List<StoredTransform>();
		leftAvatarObservations = new List<StoredTransform>();
		rightAvatarObservations = new List<StoredTransform>();
		times = new List<float>();
	}

	private List<float> TransformToPositionAndRotation(StoredTransform myStoredTransform)
	{
		List<float> concatenated = new List<float>{myStoredTransform.position.x, myStoredTransform.position.y,myStoredTransform.position.z, myStoredTransform.rotation.eulerAngles.x, myStoredTransform.rotation.eulerAngles.y, myStoredTransform.rotation.eulerAngles.z};
		return concatenated;
	}
	
//	Path.Combine(docPath, "WriteLines.txt")
	static void AddHeader(StreamWriter myWriter)
	{
		//TODO ensure block info is updated
		string header =
			"time,trial_number,condition_name,hand,block_number,participant_id,hmd_x,hmd_y,hmd_z,hmd_rot_x,hmd_rot_y,hmd_rot_z,leftController_x,leftController_y,leftController_z,leftController_rot_x,leftController_rot_y,leftController_rot_z,rightController_x,rightController_y,rightController_z,rightController_rot_x,rightController_rot_y,rightController_rot_z,leftAvatar_x,leftAvatar_y,leftAvatar_z,leftAvatar_rot_x,leftAvatar_rot_y,leftAvatar_rot_z,rightAvatar_x,rightAvatar_y,rightAvatar_z,rightAvatar_rot_x,rightAvatar_rot_y,rightAvatar_rot_z";
		myWriter.WriteLine(header);
	}

	void WriteTransform(StreamWriter outputFile, StoredTransform myStoredTransform)
	{
		List<float> myElements = TransformToPositionAndRotation(myStoredTransform);
		var myStringElements = myElements.Select(x => x+"");
		foreach (string myElement in myStringElements)
		{
			outputFile.Write(myElement + ",");
		}
		
	}
	public void SaveSignals(string filePath)
	{
//		Debug.Log("at" + Time.time + "starting save:" + filePath);
		using (StreamWriter outputFile = new StreamWriter(filePath))
		{
//			AddPreHeader(outputFile); participantId, etc
			AddHeader(outputFile);
			
			for (int i = 0; i < times.Count; i++)
			{
				outputFile.Write(times[i] + ",");
				//outputFile.Write(taskPositionsScript.trialNumber + "," + taskPositionsScript.currentConditionName + "," + RightOrLeftToString(taskPositionsScript.nameOfSuccessfulAvatar) + "," + taskPositionsScript.blockNumber + "," + participantId + ",");
				WriteTransform(outputFile, HMD_Observations[i]);
				WriteTransform(outputFile, leftControllerObservations[i]);
				WriteTransform(outputFile, rightControllerObservations[i]);
				WriteTransform(outputFile, leftAvatarObservations[i]);
				WriteTransform(outputFile, rightAvatarObservations[i]);
				outputFile.Write("\n");
			}
				
		}
//		Debug.Log("at" + Time.time + "completed saved filePath:" + filePath);
	}

}
