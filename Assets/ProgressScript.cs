using TMPro;
using UnityEngine;

public class ProgressScript : MonoBehaviour
{
    public TextMeshPro myText;
    public int frameUpdateRate = 10;
    private string initialString;
    private void Start()
    {
        myText = transform.GetChild(0).GetComponent<TextMeshPro>();
        initialString = myText.text;
    }

    private void Update()
    {
        if (Time.frameCount%frameUpdateRate==0)
        {
            myText.text = initialString + " " + transform.position.z;
        }
    }
}
