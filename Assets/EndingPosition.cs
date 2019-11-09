using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPosition : MonoBehaviour
{
    private int framesInStartCube = 0;
    public GameObject endCubeSuccessPrefab;
    
    private int debounceFrames = 100;

    private Renderer startCubeRenderer;
    private RepresentationController representationController;

    private void Start ()
    {
        representationController = GameObject.Find("HandAvatarRight").GetComponent<RepresentationController>();
        startCubeRenderer = gameObject.GetComponent<Renderer>();
        startCubeRenderer.material.color = Color.grey;
    }


    private void OnTriggerEnter(Collider other)
    {
        //ignore if in incorrect state
        if (representationController.reachingState != ReachingState.NotReaching) return;
        framesInStartCube = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if (representationController.reachingState != ReachingState.AwaitingEndPosition) return;

        if (framesInStartCube >= debounceFrames)
        {
            // task success!
            startCubeRenderer.material.color = Color.green;
            var goSignalGameObject = Instantiate(endCubeSuccessPrefab, transform);
            Destroy(goSignalGameObject, 2);
            framesInStartCube = 0;
            representationController.EndTrial();
        }
        else
        {
            // continue accruing time within the trigger
            startCubeRenderer.material.color = Color.Lerp(Color.grey, Color.yellow,
                (float) framesInStartCube / (float) debounceFrames);
            framesInStartCube += 1;
        }
		
    }

    private void OnTriggerExit(Collider other)
    {
        framesInStartCube = 0;
        startCubeRenderer.material.color = Color.grey;
    }
}
