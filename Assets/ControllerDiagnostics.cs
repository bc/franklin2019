using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerDiagnostics : MonoBehaviour
{
    // Start is called before the first frame update
    public OVRInput.Controller controller;
    public List<TextMeshPro> myTextMeshes = new List<TextMeshPro>();
    private Signals signals;
    private string formatting = "F2";
    private int framesPerUpdate = 1;
    void Start()
    {

        signals = GameObject.Find("ScriptManager").GetComponent<Signals>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Time.frameCount%framesPerUpdate==0)
        {
            var x = Signals.GetKinematics(controller);

                myTextMeshes[0].text = x.posR.ToString(formatting);
                myTextMeshes[1].text = x.velR.ToString(formatting);
                myTextMeshes[2].text = x.accR.ToString(formatting);
                myTextMeshes[3].text = x.rotREuler.ToString(formatting);
                myTextMeshes[4].text = x.rotdotR.ToString(formatting);
                myTextMeshes[5].text = x.rotdotdotR.ToString(formatting);
        }
    }
}
