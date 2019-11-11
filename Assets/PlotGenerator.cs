using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotGenerator : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject axisPrefab;
    public List<GameObject> points = new List<GameObject>();
    public List<GameObject> xAxisPoints = new List<GameObject>();

    private void Start()
    {
        PlotIt(AnimationCurve.Linear(0f,0f,1f,1f));
    }

    internal void PlotIt(AnimationCurve ac)
    {
        float[] timeBounds = new float[]{ac.keys.Min(x => x.time),ac.keys.Max(x => x.time)};
        float[] valueBounds = new float[]{ac.keys.Min(x => x.value),ac.keys.Max(x => x.value)};

        DestroyGameObjects(ref points);
        DestroyGameObjects(ref xAxisPoints);
    

        for (int i = 0; i < ac.keys.Length; i++)
        {
            var pos = new Vector3(ac.keys[i].time - timeBounds[0], ac.keys[i].value, 0f);
            points.Add(DrawPoint(pos * 5f));
            xAxisPoints.Add(DrawAxis(pos));
        }
    }

    private void DestroyGameObjects(ref List<GameObject> vec)
    {
        foreach (var x in points)
        {
            Destroy(x);
        }
        vec.Clear();
    }

    private GameObject DrawAxis(Vector3 pos)
    {
        var xAxis = Instantiate(axisPrefab, transform.position + Vector3.right * pos.x, Quaternion.identity);
        xAxis.transform.SetParent(transform);
        return xAxis;
    }

    private GameObject DrawPoint(Vector3 pos )
    {
        var point = Instantiate(pointPrefab, transform.position + pos, Quaternion.identity);
        point.transform.SetParent(transform);
        return point;
    }
}
