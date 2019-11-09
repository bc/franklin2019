using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RepresentationType
{
    Cursor,Hand
}


public enum ReachingState
{
    NotReaching,
    InitialReaching,
    PerturbedInThisFrame,
    ApplyingPerturbation,
    AwaitingEndPosition

}

public class RepresentationController : MonoBehaviour
{
    private Vector3[] cursorAndHandPos = new Vector3[2];
    private Quaternion[] cursorAndHandRot = new Quaternion[2];
    public GameObject CursorSphere;
    public GameObject HandRepresentation;
    // this is where the actual controller center always is
    public GameObject RepresentationCenterCube;
    public GameObject nearTarget;
    public GameObject farTarget;
    public static readonly float perturbationApplicationSeconds = 0.1f;
    private AnimationCurve normalizedAnimationCurve;
    private float perturbStartTime;
    private Vector3 positionPerturbationVector = Vector3.right * 0.020f;
    public ReachingState reachingState = ReachingState.NotReaching;
    private float applicationProgress;
    public float perturbProgress;
    public int trialIndex;//where each trial is a reach with given condition.
    public Condition currentCondition;
    public List<Condition> conditionsList;
    public Signals signals;
    
    private void Start()
    {
        signals = GameObject.Find("ScriptManager").GetComponent<Signals>();
        conditionsList = Condition.GenerateFullyJumbledBlockHandAlwaysPresent(1,1,0.020f,0);
        InstantiateZeroBiases();
        ResetBiases();
        normalizedAnimationCurve = AnimationCurve.EaseInOut(0.0f,0f,perturbationApplicationSeconds,1f);
        //this is the time since the last perturbation.
        currentCondition = conditionsList[0];
    }

    
   private void Update()
   {
       //this part controls what the desired offsets will be
        switch (reachingState)
        {
            case ReachingState.NotReaching:
                InstantiateZeroBiases();
                ApplyBiasesToTransforms();
                return;
            case ReachingState.InitialReaching:
                ApplyBiasesToTransforms();
                return;
            //these are the cases that will apply some measure of perturbation
            //so we don't return but just continue onto the bottom block
            case ReachingState.PerturbedInThisFrame:
                perturbStartTime = Time.time;
                perturbProgress = SetBiasByAnimation(currentCondition);
                reachingState = ReachingState.ApplyingPerturbation;
                ApplyBiasesToTransforms();
                return;
            case ReachingState.ApplyingPerturbation:
                perturbProgress = SetBiasByAnimation(currentCondition);
                if (perturbProgress >= 1.0f)
                {
                    reachingState = ReachingState.AwaitingEndPosition;
                }
                ApplyBiasesToTransforms();
                return;
            case ReachingState.AwaitingEndPosition:
                //pass, keeping the last values for the pos/rot biases.
                ApplyBiasesToTransforms();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        //this part implements them

        
        //NormalizedTaskProgress(nearTarget.transform.position,farTarget.transform.position,RepresentationCenterCube.transform.position);
   }

   private void ApplyBiasesToTransforms()
   {
       CursorSphere.transform.localPosition = cursorAndHandPos[(int)RepresentationType.Cursor];
       CursorSphere.transform.localRotation = cursorAndHandRot[(int)RepresentationType.Cursor];
       HandRepresentation.transform.localPosition = cursorAndHandPos[(int)RepresentationType.Hand];
       HandRepresentation.transform.localRotation = cursorAndHandRot[(int)RepresentationType.Hand];
   }

   private float SetBiasByAnimation(Condition _currentCondition)
   {
       applicationProgress = normalizedAnimationCurve.Evaluate(Time.time - perturbStartTime);
       SetPosBias(RepresentationType.Cursor, _currentCondition.cursorPos * applicationProgress);
       SetPosBias(RepresentationType.Hand, _currentCondition.handPos * applicationProgress);
       //TODO hand rotation
       return applicationProgress;
   }

   private void InstantiateZeroBiases()
   {
       cursorAndHandPos[0] = Vector3.zero;
       cursorAndHandPos[1] = Vector3.zero;
       cursorAndHandRot[0] = Quaternion.identity;
       cursorAndHandRot[1] = Quaternion.identity;
   }
   
   private void ResetBiases()
   {
       CursorSphere.transform.localPosition = Vector3.zero;
       CursorSphere.transform.localRotation = Quaternion.identity;
       HandRepresentation.transform.localPosition = Vector3.zero;
       HandRepresentation.transform.localRotation = Quaternion.identity;
   }

   private void SetPosBias(RepresentationType item, Vector3 inputVector)
   {
           cursorAndHandPos[(int)item] = inputVector;
   }
   
   private void SetRotBias(RepresentationType item, Quaternion inputRotation)
   {
       cursorAndHandRot[(int)item] = inputRotation;
   }


   public void EndTrial()
   {
        trialIndex += 1;
        reachingState = ReachingState.NotReaching;
        Debug.Log($"list has {conditionsList.Count} elements");
        if (trialIndex >= conditionsList.Count)
        {
            //ran out of tasks.
            return;
        }
        currentCondition = conditionsList[trialIndex];
        signals.SaveTrialResponse();
        AnimationCurve speedProfile = signals.ReachVelocityProfile();
        Debug.Log($"max val in speed was: {speedProfile.keys.Select(x=>x.value).Max()}"); 
        signals.StartNewTrialResponse();
   }
}
