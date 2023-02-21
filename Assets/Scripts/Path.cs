using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public readonly Vector3[] LookPoints;
    public readonly Line[] TurnBoundaries;
    public readonly int FinishLineIndex;

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDist)
    {
        LookPoints = waypoints;
        TurnBoundaries = new Line[LookPoints.Length];
        FinishLineIndex = TurnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);
        for (int i = 0; i < LookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(LookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = ( i == FinishLineIndex)? currentPoint  : currentPoint - dirToCurrentPoint * turnDist;
            TurnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDist);
            previousPoint = turnBoundaryPoint;
        }
    }

    Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.cyan;

        foreach (Vector3 p in LookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up,Vector3.one );
        }
        
        Gizmos.color = Color.white;
        foreach (Line l in TurnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
        
    }
    
}
