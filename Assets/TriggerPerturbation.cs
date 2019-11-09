using UnityEngine;

public class TriggerPerturbation : MonoBehaviour
{
    private Renderer myRenderer;
    private RepresentationController representationController;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        representationController = GameObject.Find("HandAvatarRight").GetComponent<RepresentationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //only accept a trigger when it is doing initial reaching.
        if (representationController.reachingState != ReachingState.InitialReaching || !other.CompareTag("CursorColliderObject"))
        {
            return;
        }
        //let the representation know that perturbation has begun

        representationController.reachingState = ReachingState.PerturbedInThisFrame;
        //make a visible change
        myRenderer.material.color = Color.green;
    }
 
}
