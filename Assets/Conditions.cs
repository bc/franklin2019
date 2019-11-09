using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Runtime.Serialization.Json;
using OVRSimpleJSON;
using UnityEngine;
using Random = System.Random;

public enum PerturbationDirection
{
    None,
    Left,
    Right
}


public enum PerturbationType
{
    DartStaticHandStaticControl,
    DartMovesHandMissing,
    DartStaticHandMoves,
    DartMovesHandStatic,
    DartMovesHandMoves,
    DartMovesHandMovesOpposite
}
[Serializable]
public struct Condition
{
    public Vector3 cursorPos;
    public Vector3 handPos;
    public int blockNumber;
    public string id;
    public Quaternion cursorRot;
    public Quaternion handRot;
    public PerturbationType perturbationType;
    public PerturbationDirection perturbationDirection;
    private static readonly Vector3 offScreen = Vector3.down * 100f; //puts it far away.
    public Condition(PerturbationType _perturbationType, PerturbationDirection _perturbationDirection, float perturbationMeters, int _blockNumber) : this()
    {
        perturbationType = _perturbationType;
        perturbationDirection = _perturbationDirection;
        Vector3 perturbVector3 = Vector3.right * GetFloatDirection(perturbationDirection) * perturbationMeters;
        id = Guid.NewGuid().ToString();
        blockNumber = _blockNumber;
        NullRotation();
        switch (_perturbationType)
        {
            case PerturbationType.DartStaticHandStaticControl:
                cursorPos = handPos = Vector3.zero;
                break;
            case PerturbationType.DartMovesHandMissing:
                cursorPos = perturbVector3;
                handPos = offScreen;
                break;
            case PerturbationType.DartStaticHandMoves:
                cursorPos = Vector3.zero;
                handPos = perturbVector3;
                break;
            case PerturbationType.DartMovesHandStatic:
                cursorPos = perturbVector3;
                handPos = Vector3.zero;
                break;
            case PerturbationType.DartMovesHandMoves:
                cursorPos = handPos = perturbVector3;
                break;
            case PerturbationType.DartMovesHandMovesOpposite:
                cursorPos =perturbVector3;
                handPos = perturbVector3*-1f; //goes opposite way
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_perturbationType), _perturbationType, null);
        }
  
    }

    private void NullRotation()
    {
        cursorRot = Quaternion.identity;
        handRot = Quaternion.identity;
    }
    private static float GetFloatDirection(PerturbationDirection dir){
        // where 1 is right
        float[] options = new float[] { 0f,-1f,1f};
        return options[(int) dir];
    }

    internal string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    /// <summary>
    ///
    /// it will shuffle all possible experimental conditions. null cases are not shown.
    /// only shows experimental conditions where the hand is visible throughout the entire experiment.
    /// </summary>
    /// <param name="perturbationDistance"></param>
    /// <param name="blockNumber"></param>
    /// <returns></returns>
    internal static List<Condition> GenerateHandVisibleExperimentalConditions(float perturbationDistance, int blockNumber)
    {
        var myList = new List<Condition>();
        //var types = Enum.GetNames(typeof(PerturbationType));
        var types = Enum.GetValues(typeof(PerturbationType)).Cast<PerturbationType>().ToList();
        var directions = Enum.GetValues(typeof(PerturbationDirection)).Cast<PerturbationDirection>().ToList();
        foreach (var type in types)
        {
            foreach (var dir in directions)
            {
                // Don't add a condition that is functionally equivalent to the null case (where hand is present).
                // don't add the static, and don't add where hand is missing.
                var is_equiv = EquivalentToControlCase(dir, type);

                if (!is_equiv)
                {
                    myList.Add(new Condition(type, dir, perturbationDistance, blockNumber));
                }
            }
        }
        return(ShuffleConditions(myList).ToList());
    }

    private static bool EquivalentToControlCase(PerturbationDirection dir, PerturbationType type)
    {
        return dir == PerturbationDirection.None ||
               type == PerturbationType.DartStaticHandStaticControl ||
               type == PerturbationType.DartMovesHandMissing;
    }

    internal static Condition GenerateControlCondition(float perturbationDistance,int blockNumber)
    {
        return new Condition(PerturbationType.DartStaticHandStaticControl, PerturbationDirection.None, perturbationDistance, blockNumber);
    }
    
    public static List<Condition> GenerateFullyJumbledBlockHandAlwaysPresent(int trialsPerControlCondition,
        int trialsPerExperimentalCondition,
        float perturbationDistance,
        int blockNumber)
    {
        var controls = Enumerable.Range(0, trialsPerControlCondition)
            .Select(x => GenerateControlCondition(perturbationDistance, blockNumber)).ToList();
        var experiments = Enumerable.Range(0, trialsPerExperimentalCondition)
            .SelectMany(x => GenerateHandVisibleExperimentalConditions(perturbationDistance, blockNumber))
            .ToList();
        controls.AddRange(collection: experiments);
        return ShuffleConditions(controls);
    }

    public static List<Condition> GenerateBlockWithHandMissing(int trialsPerControlCondition, int trialsPerExperimentalCondition,float perturbationDistance, int blockNumber)
    {
        var outList = new List<Condition>();
        for (int i = 0; i < trialsPerControlCondition; i++)
        {
            outList.Add(new Condition(PerturbationType.DartMovesHandMissing,
                PerturbationDirection.None,
                perturbationDistance,
                blockNumber));
        }
        for (int j = 0; j < trialsPerExperimentalCondition; j++)
        {
            outList.Add(new Condition(PerturbationType.DartMovesHandMissing,
                PerturbationDirection.Left,
                perturbationDistance, blockNumber));
            outList.Add(new Condition(PerturbationType.DartMovesHandMissing,
                PerturbationDirection.Right,
                perturbationDistance, blockNumber));
        }
        return outList;
    }
    
    public static List<Condition> ShuffleConditions( List<Condition> list)
    {
        var listCopy = list;
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = listCopy.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            Condition value = listCopy[k];
            listCopy[k] = listCopy[n];
            listCopy[n] = value;
        }

        return listCopy;
    }
    
}


