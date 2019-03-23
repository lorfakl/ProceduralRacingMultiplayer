using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public int PointsToGenerate = 20;
    public GameObject shape;
    public GameObject trackSegment;
    public int rectangleWidth;
    public int rectangleHeight;


    // Start is called before the first frame update
    void Start()
    {
        Rectangle boundingRect = new Rectangle(new Vector2(0, 0), rectangleHeight, rectangleWidth);
        print("made rect");
        List<Vertex> createdPts = boundingRect.GeneratePtWithin(PointsToGenerate);
        print("made " + createdPts.Count + " pts within rect");
        List<GameObject> instantedObj = new List<GameObject>();
        foreach (Vertex v in createdPts)
        {
            GameObject obj = Instantiate(shape, v.Position, Quaternion.identity);
            instantedObj.Add(obj);
        }

        print("made GameObjects");
        List<Vertex> convexHullPts = Geometry.GetConvexHull(createdPts);
        DisplayLineSegments(convexHullPts);
        print("lets hope the internet code works");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayLineSegments(List<Vertex> lineSeg)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        
        List<Vector3> vertexPosition = new List<Vector3>();

        foreach (Vertex v in lineSeg)
        {
            //print(v);
            vertexPosition.Add(v.Position);
            //vertexPosition.Add(v.Position);
        }

        lineRenderer.positionCount = vertexPosition.Count + 1;

        lineRenderer.SetPositions(vertexPosition.ToArray());
        //print("Should repeat?: " + vertexPosition[0]);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, vertexPosition[0]);
    }
}
