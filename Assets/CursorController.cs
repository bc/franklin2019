using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	public GameObject BiasZoneCube;
	public Vector3 positionBias = Vector3.zero;
	public Quaternion rotationBias = Quaternion.identity;
	public GameObject UserHand;
	public GameObject Avatar;
	public Vector3 biasVector = new Vector3(0.0f, 0.2f, 0.0f);
	private Transform currentPose;

	private void Update()
	{
		currentPose = UserHand.transform;
		Avatar.transform.SetPositionAndRotation(currentPose.position + positionBias, Avatar.transform.rotation);
	}
}
