using UnityEngine;

public class StartingPosition : MonoBehaviour
{
	private int framesInStartCube;
	public GameObject goSignalPrefab;
	private Signals signalScript;
	private int randomDebounce;

	private Renderer startCubeRenderer;
	private RepresentationController representationController;

	private void Start ()
	{
		representationController = GameObject.Find("HandAvatarRight").GetComponent<RepresentationController>();
		startCubeRenderer = gameObject.GetComponent<Renderer>();
		startCubeRenderer.material.color = Color.grey;
		signalScript = GameObject.Find("ScriptManager").GetComponent<Signals>();
	}


	private void OnTriggerEnter(Collider other)
	{
		//ignore if in incorrect state
		if (representationController.reachingState != ReachingState.NotReaching) return;

		randomDebounce = Random.Range(135, 225); //between 1.5-2.5s after trigger entry
		framesInStartCube = 0;
	}

	private void OnTriggerStay(Collider other)
	{
		if (representationController.reachingState != ReachingState.NotReaching) return;

		if (framesInStartCube >= randomDebounce)
		{
			startCubeRenderer.material.color = Color.green;
			var goSignalGameObject = Instantiate(goSignalPrefab, transform);
			Destroy(goSignalGameObject, 2);
			framesInStartCube = 0;
			signalScript.ClearOldValues();
			representationController.reachingState = ReachingState.InitialReaching;
		}
		else
		{
			// continue accruing time within the trigger
			framesInStartCube += 1;
		}
		
	}

	private void OnTriggerExit(Collider other)
	{
		framesInStartCube = 0;
		startCubeRenderer.material.color = Color.grey;
	}
}
