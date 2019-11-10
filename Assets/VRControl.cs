using UnityEngine;

public class VRControl : MonoBehaviour
{
    private Transform trackedController;
    private void Start()
    {
        trackedController = GameObject.FindGameObjectWithTag("LockToHandAvatar").transform;
    }

   private  void LateUpdate()
    {
        transform.position = trackedController.position;
        transform.rotation = trackedController.rotation;
    }
}
