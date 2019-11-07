using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiasController : MonoBehaviour
{
	private Vector3 bias;

	public float biasDirection = 1.0f;
	private float biasMagnitude = 0.040f;
	void Start () {
		bias = new Vector3(biasMagnitude,0.0f,0.0f);
	}
	void Update () {
		bias = new Vector3(biasMagnitude*biasDirection,0.0f,0.0f);
	}

	private void OnTriggerStay(Collider other)
	{
		if (Time.frameCount > 180)
		{
			if (other.transform.name != "Balloon") {
				other.transform.parent.transform.parent.transform.localPosition = bias;
			}
			
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (Time.frameCount > 180)
		{
			if (other.transform.name != "Balloon") {
				//this only happens for the temporary bias, so the sustained one happens on pop
			other.transform.parent.transform.parent.transform.localPosition = Vector3.zero;
		}}
	}
	
}
