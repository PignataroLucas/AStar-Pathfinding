using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const float MinPathUpdateTime = .2f;
    private const float PathUpdateMoveThreshold = .5f;
    
    public Transform target;
    public float speed = 5;
    public float turnSpeed = 3;
    public float stopDistance = 10f;
    //private Vector3[] path;
    //private int targetIndex;
    public float turnDistance = 4f;

    private Path _path;
    
    

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);
        StartCoroutine(UpdatePath());
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {   
            //path = newPath;
            //targetIndex = 0;
            _path = new Path(newPath, transform.position, turnDistance,stopDistance);
            StopCoroutine("FollowPath"); 
            StartCoroutine("FollowPath"); 
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        
        PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);
        float srqMoveThreshold = PathUpdateMoveThreshold * PathUpdateMoveThreshold;
        Vector3 targetPositionOld = target.position;
        
        while (true)
        {
            yield return new WaitForSeconds(MinPathUpdateTime);
            if ((target.position - targetPositionOld).sqrMagnitude > srqMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);
                targetPositionOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        //Vector3 currentWaypoint = path[0];
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_path.LookPoints[0]);

        float speedPercent = 1f;
        
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
                if (pathIndex >= _path.SlowDownIndex && stopDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(_path.TurnBoundaries[_path.FinishLineIndex].DistanceFromPoint(pos2D) / stopDistance);
                    //transform.LookAt(target.position);
                    if (speedPercent < 0.01f)
                    {
                        //Maybe do the look at here
                        followingPath = false;
                    }
                }
                Quaternion targetRotation = Quaternion.LookRotation(_path.LookPoints[pathIndex]-transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * (Time.deltaTime * speed * speedPercent) , Space.Self);
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
