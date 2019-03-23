using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle
{
    private Vector2 startingPt;
    private int length;
    private int width;

    public Rectangle(Vector2 startingPt, int l, int w)
    {
        this.startingPt = startingPt;
        length = l;
        width = w;
    }

    public List<Vertex> GeneratePtWithin(int num)
    {
        List<Vertex> createdPts = new List<Vertex>();

        for (int i = 0; i < num; i++)
        {
            createdPts.Add(new Vertex(CreateRandomPosition()));
        }

        return createdPts;
    }

    private Vector3 CreateRandomPosition()
    {
        float x = Random.Range(0, length);
        float y = Random.Range(0, width);

        return new Vector3(x, y, 0);
    }
}
