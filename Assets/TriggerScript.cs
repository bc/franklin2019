using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
	public GameObject ScriptManager;
	private CursorController playerScript;
	private void Start()
	{

		playerScript =  GameObject.Find("ScriptManager").GetComponent<CursorController>();
	}

//	private void OnTriggerEnter(Collider other)
//	{
//
//	
//	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("exited");
		playerScript.positionBias = Vector3.zero;
	}

	private void OnTriggerStay(Collider other)
	{
		Debug.Log("within");
		playerScript.positionBias = playerScript.biasVector;
	}
}
