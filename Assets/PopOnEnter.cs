using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnEnter : MonoBehaviour
{
	public GameObject prefabExplosion;
	public float explosionSize = 1.0f;
	private Renderer targetRenderer;
	private GameObject newExplosion;
	private AdjustPositions taskPositionsScript;
	private Signals signalScript;
	public GameObject switchHandsPrefab;
	private int framesWithin = 0;
	private void Start()
	{
		targetRenderer = gameObject.GetComponent<Renderer>();
		taskPositionsScript = this.GetComponentInParent<AdjustPositions>();
		targetRenderer.material.color = Color.cyan;
		signalScript = GameObject.Find("SignalRecording").GetComponent<Signals>();
	}

	private void OnTriggerStay(Collider other)
	{
		
		if (other.CompareTag("BiasZoneTag"))
		{
//			Debug.Log("bias zone disallowed from triggering target");
			//do not allow the bias zone to trigger the target.
			return;
		}
	targetRenderer.material.color = Color.green;
	framesWithin += 1;
		if (taskPositionsScript.taskState == "taskBegun" & framesWithin > 45)
		{
//			Debug.Log("has entered target");
			taskPositionsScript.taskState = "taskCompleted";
			framesWithin = 0;
			targetRenderer.material.color = Color.cyan;
			newExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
			Destroy(newExplosion,2);
			newExplosion.transform.SetParent(transform);
			// we don't want the user to observe a perturbation when they return their hand, so we hide it,
			// until they're back at the beginning location
			taskPositionsScript.nameOfSuccessfulAvatar = other.gameObject.transform.parent.gameObject.transform.parent.name;
			signalScript.StoreTrialData(taskPositionsScript.blockNumber,taskPositionsScript.trialNumber,taskPositionsScript.nameOfSuccessfulAvatar);
			// TODO make this flexible with the input condition condition array
			//increment trial by 1
			taskPositionsScript.trialNumber += 1;
			if (taskPositionsScript.trialNumber == 6)
			{
				//rollover
				taskPositionsScript.trialNumber = 0;
				taskPositionsScript.blockNumber += 1;
				if (taskPositionsScript.blockNumber == 5)
				{
					//rollover
					taskPositionsScript.blockNumber = 0;
					GameObject switchHandsNotification = Instantiate(switchHandsPrefab);
					Destroy(switchHandsNotification, 2);
				}
			}
			Debug.Log("Next: BLOCK:" + taskPositionsScript.blockNumber + "TRIAL:" + taskPositionsScript.trialNumber);
			framesWithin = 0;
		}
	}


	private void OnTriggerExit(Collider other)
	{
		targetRenderer.material.color = Color.grey;
		framesWithin = 0;
	}
}
