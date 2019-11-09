using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCondition : MonoBehaviour
{
    private TextMeshPro tmpp;
    private TextMeshPro number;
    private RepresentationController rc;
    void Start()
    {
        tmpp=transform.GetChild(0).GetComponent<TextMeshPro>();
        number=transform.GetChild(1).GetComponent<TextMeshPro>();
        rc = GameObject.Find("HandAvatarRight").GetComponent<RepresentationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount%10!=0)
        {
            return;
        }
        var direction = rc.currentCondition.perturbationDirection.ToString();
        var type = rc.currentCondition.perturbationType.ToString();
        tmpp.text = $"{type}\n{direction}";
        number.text = $"B:{rc.currentCondition.blockNumber}\n{rc.trialIndex}/{rc.conditionsList.Count}\n{rc.currentCondition.id}";
    }
}

