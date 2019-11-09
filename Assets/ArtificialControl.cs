using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialControl : MonoBehaviour
{
    public Transform[] closeFarTargets;
    private int targetIndex;
    public Transform cursorCollider;
    private RepresentationController rc;
    private Vector3 priorCommand = Vector3.zero;
   private  void Start()
    {
        rc=GetComponent<RepresentationController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (rc.reachingState)
        {
            case ReachingState.NotReaching:
                targetIndex = 0;
                break;
            case ReachingState.InitialReaching:
                targetIndex = 1;
                break;
            case ReachingState.PerturbedInThisFrame:
                targetIndex = 1;
                break;
            case ReachingState.ApplyingPerturbation:
                targetIndex = 1;
                break;
            case ReachingState.AwaitingEndPosition:
                targetIndex = 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var taskDisparity = closeFarTargets[targetIndex].position - cursorCollider.position;
        var newCommand = (taskDisparity * 0.01f + priorCommand)/2f;
        transform.position += newCommand;
        priorCommand = newCommand;
    }
}
