using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    private TextMesh myText;
    private int counter = 0;
    private AdjustPositions taskPositionsScript;
    // Start is called before the first frame update
    void Start()
    {
        myText = this.GetComponent<TextMesh>();
        taskPositionsScript = GameObject.Find("TaskContainer").GetComponent<AdjustPositions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 45)
        {
               myText.text = String.Format("Block {0}, Trial {1}",taskPositionsScript.blockNumber,taskPositionsScript.trialNumber);
            counter = 0;
        }

        counter += 1;

    }
}
