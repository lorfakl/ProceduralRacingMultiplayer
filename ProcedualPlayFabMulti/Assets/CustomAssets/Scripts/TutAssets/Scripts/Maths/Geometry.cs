using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Geometry 
{
    /// <summary>
    /// Angle between two Vertexes, uses the the vertex with minimum Y value as the horizontal by which to
    /// measure the angle
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns>Angle in degrees</returns>
    public static float Angle(Vertex p1, Vertex p2)
    {
        float angle = Mathf.Atan2(p2.Y - p1.Y, p2.X - p1.X) * Mathf.Rad2Deg;
        return angle;
    }

    public static List<LineSegment> CalculateConvexHull(List<Vertex> points)
    {
        if(points.Count < 3)
        {
            return null;
        }

        List<Vertex> ptsOfConvexHull = new List<Vertex>();

        int leftMost = 0;
        for(int i = 0; i < points.Count; i++)
        {
            if(points[i].X < points[leftMost].X)
            {
                leftMost = i;
            }
        }

        //Vertex startPt = points[leftMost];
        //int
        int p = leftMost;
        Debug.Log("Starting Jarvis March");

        do
        {
            ptsOfConvexHull.Add(points[p]);

            int q = (p + 1) % points.Count;
            for (int i = 0; i < points.Count; i++)
            {
            Debug.Log("In the loop");
                Orientation o = points[p].GetOrientation(points[i], points[q]);
                if (o == Orientation.CCW)
                {
                    q = i;
                }
            }

            p = q;

            if(p == leftMost)
            {
                Debug.Log("is this ever true?");
                break;
            }

        }while (p != leftMost);

        List<LineSegment> convexHull = new List<LineSegment>();

        for(int i = 0; i < ptsOfConvexHull.Count; i++)
        {
            if(i == (ptsOfConvexHull.Count - 1))
            {
                Vertex first = ptsOfConvexHull[0];
                Vertex last = ptsOfConvexHull[ptsOfConvexHull.Count - 1];
                i = 0;
                convexHull.Add(new LineSegment(last, first, last.Distance(first)));
                break;
            }
            Vertex a = ptsOfConvexHull[i];
            Vertex b = ptsOfConvexHull[i + 1];
            
            convexHull.Add(new LineSegment(a, b, a.Distance(b)));
        }

        return convexHull;
    }

    public static List<Vertex> convexHull(List<Vertex> points, int n)
    {
        // There must be at least 3 points 
        if (n < 3) return null;

        // Initialize Result 
        List<Vertex> hull = new List<Vertex>();

        // Find the leftmost point 
        int l = 0;
        for (int i = 1; i < n; i++)
            if (points[i].X < points[l].X)
                l = i;

        // Start from leftmost point, keep moving counterclockwise 
        // until reach the start point again.  This loop runs O(h) 
        // times where h is number of points in result or output. 
        int p = l, q;
        do
        {
            // Add current point to result 
            hull.Add(points[p]);

            // Search for a point 'q' such that orientation(p, x, 
            // q) is counterclockwise for all points 'x'. The idea 
            // is to keep track of last visited most counterclock- 
            // wise point in q. If any point 'i' is more counterclock- 
            // wise than q, then update q. 
            q = (p + 1) % n;
            for (int i = 0; i < n; i++)
            {
                // If i is more counterclockwise than current q, then 
                // update q 
                if (Vertex.GetOrientation(points[p], points[i], points[q]) == Orientation.CCW)
                    q = i;
            }

            // Now q is the most counterclockwise with respect to p 
            // Set p as q for next iteration, so that q is added to 
            // result 'hull' 
            p = q;

        } while (p != l);  // While we don't come to first point 

        // Debug.Log Result 
        for (int i = 0; i < hull.Count; i++)
        {
            Debug.Log("(" + hull[i].X + ", "+ hull[i].Y + ")\n");
        }

        return hull;
    }

    public static List<Vertex> GetConvexHull(List<Vertex> points)
    {
        //If we have just 3 points, then they are the convex hull, so return those
        if (points.Count == 3)
        {
            //These might not be ccw, and they may also be colinear
            return points;
        }

        //If fewer points, then we cant create a convex hull
        if (points.Count < 3)
        {
            return null;
        }



        //The list with points on the convex hull
        List<Vertex> convexHull = new List<Vertex>();

        //Step 1. Find the vertex with the smallest x coordinate
        //If several have the same x coordinate, find the one with the smallest z
        Vertex startVertex = points[0];

        Vector3 startPos = startVertex.Position;

        for (int i = 1; i < points.Count; i++)
        {
            Vector3 testPos = points[i].Position;

            //Because of precision issues, we use Mathf.Approximately to test if the x positions are the same
            if (testPos.x < startPos.x || (Mathf.Approximately(testPos.x, startPos.x) && testPos.y < startPos.y))
            {
                startVertex = points[i];

                startPos = startVertex.Position;
            }
        }

        //This vertex is always on the convex hull
        convexHull.Add(startVertex);

        points.Remove(startVertex);



        //Step 2. Loop to generate the convex hull
        Vertex currentPoint = convexHull[0];

        //Store colinear points here - better to create this list once than each loop
        List<Vertex> colinearPoints = new List<Vertex>();

        int counter = 0;

        while (true)
        {
            //After 2 iterations we have to add the start position again so we can terminate the algorithm
            //Cant use convexhull.count because of colinear points, so we need a counter
            if (counter == 2)
            {
                points.Add(convexHull[0]);
            }

            //Pick next point randomly
            Vertex nextPoint = points[Random.Range(0, points.Count)];

            //To 2d space so we can see if a point is to the left is the vector ab
            Vector2 a = currentPoint.GetXYPosition();

            Vector2 b = nextPoint.GetXYPosition();

            //Test if there's a point to the right of ab, if so then it's the new b
            for (int i = 0; i < points.Count; i++)
            {
                //Dont test the point we picked randomly
                if (points[i].Equals(nextPoint))
                {
                    continue;
                }

                Vector2 c = points[i].GetXYPosition();

                //Where is c in relation to a-b
                // < 0 -> to the right
                // = 0 -> on the line
                // > 0 -> to the left
                float relation = Geometry.IsAPointLeftOfVectorOrOnTheLine(a, b, c);

                //Colinear points
                //Cant use exactly 0 because of floating point precision issues
                //This accuracy is smallest possible, if smaller points will be missed if we are testing with a plane
                float accuracy = 0.00001f;

                if (relation < accuracy && relation > -accuracy)
                {
                    colinearPoints.Add(points[i]);
                }
                //To the right = better point, so pick it as next point on the convex hull
                else if (relation < 0f)
                {
                    nextPoint = points[i];

                    b = nextPoint.GetXYPosition();

                    //Clear colinear points
                    colinearPoints.Clear();
                }
                //To the left = worse point so do nothing
            }



            //If we have colinear points
            if (colinearPoints.Count > 0)
            {
                colinearPoints.Add(nextPoint);

                //Sort this list, so we can add the colinear points in correct order
                colinearPoints = colinearPoints.OrderBy(n => Vector3.SqrMagnitude(n.Position - currentPoint.Position)).ToList();

                convexHull.AddRange(colinearPoints);

                currentPoint = colinearPoints[colinearPoints.Count - 1];

                //Remove the points that are now on the convex hull
                for (int i = 0; i < colinearPoints.Count; i++)
                {
                    points.Remove(colinearPoints[i]);
                }

                colinearPoints.Clear();
            }
            else
            {
                convexHull.Add(nextPoint);

                points.Remove(nextPoint);

                currentPoint = nextPoint;
            }

            //Have we found the first point on the hull? If so we have completed the hull
            if (currentPoint.Equals(convexHull[0]))
            {
                //Then remove it because it is the same as the first point, and we want a convex hull with no duplicates
                convexHull.RemoveAt(convexHull.Count - 1);

                break;
            }

            counter += 1;
        }

        return convexHull;
    }

    public static float IsAPointLeftOfVectorOrOnTheLine(Vector2 a, Vector2 b, Vector2 p)
    {
        float determinant = (a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x);

        return determinant;
    }
}
