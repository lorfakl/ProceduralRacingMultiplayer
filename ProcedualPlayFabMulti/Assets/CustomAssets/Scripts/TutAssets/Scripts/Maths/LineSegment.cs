using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment 
{
    private readonly Vertex v1, v2;
    private LineSegment nextSegment;
    private LineSegment prevSegment;
    private float length;
    private float angle;

    public float Angle
    {
        get { return angle; }
    }

    public float Length
    {
        get { return length; }
    }

    public Vertex V1
    {
        get { return v1; }
    }

    public Vertex V2
    {
        get { return v2; }
    }

    public LineSegment NextSegment
    {
        get { return nextSegment; }
        set { nextSegment = value; }
    }

    public LineSegment PrevSegment
    {
        get { return prevSegment; }
        set { prevSegment = value; }
    }

    public LineSegment(Vertex v1, Vertex v2, float length)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.length = length;
        angle = Geometry.Angle(v1, v2);
    }

    public static List<Vertex> GetVertices(List<LineSegment> lineSeg)
    {
        List<Vertex> vertices = new List<Vertex>();
        foreach(LineSegment ls in lineSeg)
        {
            vertices.Add(ls.V1);
            vertices.Add(ls.V2);
        }

        return vertices;
    }

    public static bool CheckIntersection(LineSegment p, LineSegment q)
    {
        Vertex p1 = p.V1;
        Vertex p2 = p.V2;
        Vertex q1 = q.V1;
        Vertex q2 = q.V2;

        Orientation o1 = Vertex.GetOrientation(p1, q1, p2);
        Orientation o2 = Vertex.GetOrientation(p1, q1, q2);
        Orientation o3 = Vertex.GetOrientation(p2, q2, q1);
        Orientation o4 = Vertex.GetOrientation(p2, q2, p1);

        if((o1 != o2) && (o3 != o4))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
