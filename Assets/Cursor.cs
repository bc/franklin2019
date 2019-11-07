using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
	public GameObject prefabForCursor;
	public float blindnessThresholdZ = 0.05f;
	private GameObject cursorPrefab;
	private Quaternion cursorRotation = Quaternion.identity;
	private AdjustPositions taskPositionsScript;
	// Use this for initialization
	void Start ()
	{
		cursorPrefab = Instantiate(prefabForCursor, transform);
		cursorPrefab.transform.parent = transform;
		cursorPrefab.transform.localPosition = Vector3.zero;
		GameObject taskContainerObject = GameObject.FindGameObjectsWithTag("taskContainerTag")[0];
		taskPositionsScript = taskContainerObject.GetComponent<AdjustPositions>();
		cursorPrefab.name = this.name + "avatar";

	}
	
	// Update is called once per frame
	void Update ()
	{
		cursorPrefab.transform.rotation = cursorRotation;
		if (taskPositionsScript.taskState == "taskCompleted" & transform.position.z > blindnessThresholdZ)
		{
			cursorPrefab.SetActive(false);
		} else if (taskPositionsScript.taskState == "taskCompleted")
		{
			cursorPrefab.transform.localPosition = Vector3.zero;
			cursorPrefab.SetActive(true);
		}
		else
		{
			cursorPrefab.SetActive(true);
		}
		
	}
}
