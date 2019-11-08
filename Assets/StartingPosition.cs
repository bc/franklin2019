using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPosition : MonoBehaviour
{
	private int framesInStartCube = 0;
	public GameObject goSignalPrefab;
	private Signals signalScript;
	private int randomDebounce;

	private Renderer startCubeRenderer;

	private void Start () {
		startCubeRenderer = gameObject.GetComponent<Renderer>();
		startCubeRenderer.material.color = Color.grey;
		signalScript = GameObject.Find("ScriptManager").GetComponent<Signals>();
	}


	private void OnTriggerEnter(Collider other)
	{
		randomDebounce = UnityEngine.Random.Range(135, 225); //between 1.5-2.5s after trigger entry
	}

	private void OnTriggerStay(Collider other)
	{
// TODO makesure task hasn't already started
		if (framesInStartCube == randomDebounce)
		{
			startCubeRenderer.material.color = Color.green;
			GameObject goSignalGameObject = Instantiate(goSignalPrefab, transform);
			Destroy(goSignalGameObject, 2);
			framesInStartCube = 0;
			Debug.Log("taskBegun");
			signalScript.ClearOldValues();
			//TODO change to next task in queue
		}
		framesInStartCube += 1;
	}

	private void OnTriggerExit(Collider other)
	{
		framesInStartCube = 0;
		startCubeRenderer.material.color = Color.grey;
	}
}
