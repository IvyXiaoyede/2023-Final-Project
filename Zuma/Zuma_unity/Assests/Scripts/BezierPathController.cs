using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class BezierPathController : MonoBehaviour
{
    public bool Debug = true;
    public GameObject ballPrefab;
    public float BallAndBallDis;
    public int segmentsPerCurve;
    public List<GameObject> ControlPointList = new List<GameObject>();
    public List<Vector3> pathPointList = new List<Vector3>();

    private void Awake()
    {
        foreach (var item in pathPointList)
        {
            GameObject ball = Instantiate(ballPrefab, GameObject.Find("Map").transform);
            ball.transform.position = item;
        }
    }
    private void OnDrawGizmos()
    {
        ControlPointList.Clear();
        foreach (Transform item in transform)
        {
            ControlPointList.Add(item.gameObject);
        }
        List<Vector3> controlPointPos = ControlPointList.Select(point => point.transform.position).ToList();
        var points = GetDrawingPoints(controlPointPos, segmentsPerCurve);

        Vector3 startPos = points[0];
        pathPointList.Clear();
        pathPointList.Add(startPos);
        for (int i = 1; i < points.Count; i++)
        {
            if (Vector3.Distance(startPos, points[i]) >= BallAndBallDis)
            {
                startPos = points[i];
                pathPointList.Add(startPos);
            }
        }

        foreach (var item in ControlPointList)
        {
            item.GetComponent<MeshRenderer>().enabled = Debug;
        }
        if (Debug == false) return;
        Gizmos.color = Color.blue;
        foreach (var item in pathPointList)
        {
            Gizmos.DrawSphere(item, BallAndBallDis / 2);
        }
        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
        //绘制贝塞尔曲线控制点连线，红色
        Gizmos.color = Color.red;
        for (int i = 0; i < controlPointPos.Count - 1; i++)
        {
            Gizmos.DrawLine(controlPointPos[i], controlPointPos[i + 1]);
        }

    }
    public List<Vector3> GetDrawingPoints(List<Vector3> controlPoints, int segmentsPerCurve)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < controlPoints.Count - 3; i += 3)
        {
            var p0 = controlPoints[i];
            var p1 = controlPoints[i + 1];
            var p2 = controlPoints[i + 2];
            var p3 = controlPoints[i + 3];

            for (int j = 0; j <= segmentsPerCurve; j++)
            {
                var t = j / (float)segmentsPerCurve;
                points.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
            }
        }
        return points;
    }
    public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var x = 1 - t;
        var xx = x * x;
        var xxx = x * x * x;
        var tt = t * t;
        var ttt = t * t * t;
        return p0 * xxx + 3 * p1 * t * xx + 3 * p2 * tt * x + p3 * ttt;
    }

    public void CreateMapAsset()
    {
        string assetPath = "Assets/Map/map.asset";
        MapConfig mapConfig = new MapConfig();
        foreach (var item in pathPointList)
        {
            mapConfig.pathPointList.Add(item);
        }
        AssetDatabase.CreateAsset(mapConfig, assetPath);
        AssetDatabase.SaveAssets();
    }
}
[CustomEditor(typeof(BezierPathController))]
public class BezierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("生成地图文件"))
        {
            (target as BezierPathController).CreateMapAsset();
        }
    }
}