using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AdjustPositions : MonoBehaviour
{
	private Transform startingPositionCube;
	private Transform biasCubeTransform;
	private Transform targetPosition;
	public float reachDelta;
	public string taskState = "initialization";
	public float biasCenterFraction = 0.5f;
	private int taskCounter = 0;
	private float targetPositionZ = 0.40f;
	private float widthFraction = 0.40f;
	private List<string> conditionNames = new List<string>
		{"temporary_L", "temporary_R", "sustained_L", "sustained_R", "null_case", "null_case"};

	public int numTrialsPerBlock = 5;
	public int numBlocks = 6;
	private Vector3 hiddenPosition;
	public int[,] blockArray = new int[5, 6]
		{{4, 2, 1, 5, 0, 3}, {1, 2, 3, 0, 5, 4}, {0, 3, 1, 2, 5, 4}, {1, 4, 3, 0, 5, 2}, {5, 2, 4, 0, 1, 3}};
	public int blockNumber = 4;
	public int trialNumber = 5;

	private List<string> blockConditionNames;
	private BiasController biasControllerScript;
	public string currentConditionName;
	public string nameOfSuccessfulAvatar;
	private Vector3 biasCenter;
	// Use this for initialization
	void Start ()
	{
		taskState = "initialization";
		//bring in children pointers
		startingPositionCube = gameObject.transform.GetChild(0);
		targetPosition = gameObject.transform.GetChild(1);
		biasCubeTransform = gameObject.transform.GetChild(2);
		biasControllerScript = biasCubeTransform.GetComponent<BiasController>();
		// center starting cube at center position
		startingPositionCube.transform.localPosition = Vector3.zero;
		targetPosition.transform.localPosition = new Vector3(0.0f,0.0f,targetPositionZ);
		Debug.Log("startingPositionCube location: " + startingPositionCube.transform.position);
		Debug.Log("targetPosition location: " + targetPosition.transform.position);
		hiddenPosition = biasCubeTransform.transform.position + Vector3.down * -5.0f;
		ArrangeCondition(blockNumber, trialNumber);

	}
	
	public void ArrangeCondition(int block, int trial)
	{
		int conditionIndex = blockArray[block, trial];
		currentConditionName = conditionNames[conditionIndex];
//		Debug.Log("Condition index is " + conditionIndex + "which means its time for " + currentConditionName + "Block: " + blockNumber + "trial: " + trialNumber);
		if (currentConditionName == "temporary_L")
		{
			ArrangeTemporaryBias(1.0f);
		}
		  else if (currentConditionName == "temporary_R")
		{
			ArrangeTemporaryBias(-1.0f);
		} else if (currentConditionName == "sustained_L")
		{
			ArrangeSustainedBias(1.0f);
		}else if (currentConditionName == "sustained_R")
		{
			ArrangeSustainedBias(-1.0f);
		} else if (currentConditionName == "null_case")
		{
			ArrangeNullBias();
		}
		else
		{
			Debug.Log("ERROR, CASE DIDN'T MATCH KNOWN CONDITION");
		}

		
	}
	void ArrangeTemporaryBias(float biasDirection)
	{
		biasCenter = Vector3.Lerp(startingPositionCube.position, targetPosition.position,biasCenterFraction);
		biasCubeTransform.transform.position = biasCenter;
		reachDelta = Vector3.Distance(startingPositionCube.position, targetPosition.position);
		biasCubeTransform.transform.localScale = new Vector3(1.0f,1.0f,reachDelta*widthFraction);
		biasControllerScript.biasDirection = biasDirection;
	}

	void ArrangeNullBias()
	{
		biasCubeTransform.transform.position = biasCenter + Vector3.down*10f;
	}
	
	void ArrangeSustainedBias(float biasDirection)
	{
		biasCubeTransform.transform.position = targetPosition.position;;
		reachDelta = Vector3.Distance(startingPositionCube.position, targetPosition.position); //0.4
		biasCubeTransform.transform.localScale = new Vector3(1.0f,1.0f,0.6f);
		biasControllerScript.biasDirection = biasDirection;
	}


	void HideBias()
	{
		biasCubeTransform.transform.position = hiddenPosition;
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log("Task State:" + taskState);
		// TODO limit task time to 2 seconds
		if (taskState == "taskCompleted" | taskState == "initialization")
		{
			HideBias();
		}
	}
}
