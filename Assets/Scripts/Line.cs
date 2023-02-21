using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    private const float VerticalLineGradient = 1e5f;
    
    private float _gradient;
    private float _yIntercept;
    private Vector2 _pointOnLine1;
    private Vector2 pointOnLine_2;
    
    
    private float _gradientPerpendicular;

    private bool _approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if (dx == 0)
        {
            _gradientPerpendicular = VerticalLineGradient;
        }
        else
        {
            _gradientPerpendicular = dy / dx;    
        }
        if (_gradientPerpendicular == 0)
        {
            _gradient = VerticalLineGradient;
        }
        else
        {
            _gradient = -1 / _gradientPerpendicular;  
        }
        _yIntercept = pointOnLine.y - _gradient * pointOnLine.x;
        _pointOnLine1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, _gradient);

        _approachSide = false;
        _approachSide = GetSide(pointPerpendicularToLine);
    }

    bool GetSide(Vector2 p)
    {
        return (p.x - _pointOnLine1.x) * (pointOnLine_2.y - _pointOnLine1.y) >
               (p.y - _pointOnLine1.y) * (pointOnLine_2.x - _pointOnLine1.x); 
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != _approachSide;
    }

    public void DrawWithGizmos(float lenght)
    {
        Vector3 lineDir = new Vector3(1, 0, _gradient).normalized;
        Vector3 lineCenter = new Vector3(_pointOnLine1.x, 0, _pointOnLine1.y)+Vector3.up;
        Gizmos.DrawLine(lineCenter - lineDir * lenght / 2f ,lineCenter + lineDir * lenght / 2f);
    }
    
}
