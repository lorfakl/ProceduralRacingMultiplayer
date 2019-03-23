using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis { X, Y , Z};
public enum Orientation { CW, CCW, Colinear};

public class Vertex 
{
    private Vector3 position;
    private LineSegment segment;
    
    public float X
    {
        get { return position.x; }
    }

    public float Y
    {
        get { return position.y; }
    }
    public Vector3 Position
    {
        get { return position; }
    }

    public Vertex(Vector3 position)
    {
        this.position = position;
        segment = null;
    }

    public Vector2 GetXYPosition()
    {
        return new Vector2(position.x, position.y);
    }

    public string Prnt()
    {
        string s = "(" + position.x + ", " + position.y + ")";
        return s;
    }

    public Orientation GetOrientation(Vertex b, Vertex c)
    {
        return Vertex.GetOrientation(this, b, c);
    }

    public static Orientation GetOrientation(Vertex a, Vertex b, Vertex c)
    {
        float val = (b.Y - a.Y) * (c.X - b.X) -
              (b.X - a.X) * (c.Y - b.Y);

        if(val == 0)
        {
            return Orientation.Colinear;
        }
        else if(val == 1)
        {
            return Orientation.CW;
        }
        else
        {
            return Orientation.CCW;
        }
    }

    public float Distance(Vertex b)
    {
        return Vector3.Distance(this.position, b.position);
    }

    public static float Distance(Vertex a, Vertex b)
    {
        return Vector3.Distance(a.position, b.position);
    }
}
