using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RepresentationType
{
    Hand,Avatar
}

public class RepresentationController : MonoBehaviour
{
    public Vector3[] avatarAndHandPos;
    public Quaternion[] avatarAndHandRot;
    public GameObject AvatarSphere;
    public GameObject HandRepresentation;
    // this is where the actual controller center always is
    public GameObject RepresentationCenterCube;
    public GameObject nearTarget;
    public GameObject farTarget;
    public GameObject lineOfActionProgress;
    private void Start()
    {
        ResetBiases();
    }

   private void Update()
    {
        AvatarSphere.transform.localPosition = avatarAndHandPos[0];
        AvatarSphere.transform.localRotation = avatarAndHandRot[0];
        HandRepresentation.transform.localPosition = avatarAndHandPos[1];
        HandRepresentation.transform.localRotation = avatarAndHandRot[1];
        NormalizedTaskProgress(nearTarget.transform.position,farTarget.transform.position,RepresentationCenterCube.transform.position);
    }

   private void NormalizedTaskProgress(Vector3 nearPosition, Vector3 farPosition, Vector3 controllerPosition)
   {
       var lineOfAction = farPosition - nearPosition;
       var currentLineProgress = Vector3.Project(controllerPosition - nearPosition, lineOfAction);
       lineOfActionProgress.transform.position = currentLineProgress;
       Debug.DrawLine(nearPosition,farPosition,Color.red);
       Debug.DrawLine(nearPosition,controllerPosition,Color.blue);
       Debug.DrawLine(controllerPosition,currentLineProgress, Color.white);
       Debug.DrawLine(controllerPosition,nearPosition + currentLineProgress, Color.green);
       
       
   }

   internal void ResetBiases()
   {
       avatarAndHandPos = new Vector3[2] {Vector3.zero, Vector3.zero};
       avatarAndHandRot =  new Quaternion[2] {Quaternion.identity, Quaternion.identity};
   }

   internal void SetPosBias(RepresentationType item, Vector3 inputVector)
   {
           avatarAndHandPos[(int)item] = inputVector;
   }
   
   internal void SetRotBias(RepresentationType item, Quaternion inputRotation)
   {
       avatarAndHandRot[(int)item] = inputRotation;
   }
   
   
}
