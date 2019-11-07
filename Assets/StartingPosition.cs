using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPosition : MonoBehaviour
{
	private int framesInStartCube = 0;
	public GameObject goSignalPrefab;
	private AdjustPositions taskPositionsScript;
	private Signals signalScript;
	private int randomDebounce;

	private Renderer startCubeRenderer;
	// Use this for initialization
	void Start () {
		startCubeRenderer = gameObject.GetComponent<Renderer>();
		startCubeRenderer.material.color = Color.grey;
		taskPositionsScript = this.GetComponentInParent<AdjustPositions>();
		signalScript = GameObject.Find("SignalRecording").GetComponent<Signals>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter(Collider other)
	{
		randomDebounce = UnityEngine.Random.Range(135, 225); //between 1.5-2.5s after trigger entry
	}

	private void OnTriggerStay(Collider other)
	{

		if (framesInStartCube == randomDebounce & taskPositionsScript.taskState != "taskBegun")
		{
			startCubeRenderer.material.color = Color.green;
			GameObject goSignalGameObject = Instantiate(goSignalPrefab, transform);
			Destroy(goSignalGameObject, 2);
			framesInStartCube = 0;
//			Debug.Log("Arranging task now; please start movement reach. StartPoint:" + Time.time);
			taskPositionsScript.taskState = "taskBegun";
			signalScript.ClearOldValues();
			taskPositionsScript.ArrangeCondition(taskPositionsScript.blockNumber, taskPositionsScript.trialNumber);
		}
		framesInStartCube += 1;
	}

	private void OnTriggerExit(Collider other)
	{
		framesInStartCube = 0;
		startCubeRenderer.material.color = Color.grey;
	}
}
