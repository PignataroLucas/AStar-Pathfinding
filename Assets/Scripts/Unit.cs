using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    public float turnSpeed = 3;
    //private Vector3[] path;
    //private int targetIndex;
    public float turnDistance = 4f;

    private Path _path;
    
    

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {    //path = newPath;
            //targetIndex = 0;
            _path = new Path(newPath, transform.position, turnDistance);
            StopCoroutine("FollowPath"); 
            StartCoroutine("FollowPath"); 
        }
    }

    IEnumerator FollowPath()
    {
        //Vector3 currentWaypoint = path[0];
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_path.LookPoints[0]); 
        
        while (followingPath)
        {
           
            /*if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    //Debug.Log("Llegue");
                    yield break;
                }
                currentWaypoint = path[targetIndex]; 
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);*/

            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (_path.TurnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == _path.FinishLineIndex)
                {
                    //we finish the path
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }
            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_path.LookPoints[pathIndex]-transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed , Space.Self);
            }
            yield return null; 
        }
    }

    private void OnDrawGizmos()
    {
        if (_path != null)
        {
            _path.DrawWithGizmos();
            /*for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position,path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1],path[i]);
                }
            }*/
        }
    }
}
