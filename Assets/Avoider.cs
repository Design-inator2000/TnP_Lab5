using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Avoider : MonoBehaviour
{
    NavMeshAgent navAgent;

    [SerializeField]
    bool showGizmo = true;
    [SerializeField]
    Transform targetToAvoid;
    [SerializeField]
    float range;
    [SerializeField]
    float speed;
    [SerializeField]
    float poissonDiscHeight, poissonDiscWidth;
    List<Vector3> localPoissonPoints=new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        PoissonGenerator();

        navAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> safePoints = new List<Vector3>();

        navAgent.speed = speed;

        if (LineOfSightCheck(this.transform.position))
        {

            if (showGizmo == true)
            {
                //var points= localPoissonPoints.ToList().Select(e=>LineOfSightCheck(this.transform.TransformPoint(e)));
                foreach (var point in localPoissonPoints)
                {

                    if (!LineOfSightCheck(this.transform.TransformPoint(point)))
                    {
                        Debug.DrawLine(this.transform.position, this.transform.TransformPoint(point), Color.green);
                        safePoints.Add(this.transform.TransformPoint(point));
                    }
                    else
                    {
                        Debug.DrawLine(this.transform.position, this.transform.TransformPoint(point), Color.red);
                    }

                }
            }

            Vector3 closestPoint = safePoints[0];

            foreach (var point in safePoints)
            {
                if (Vector3.Distance(point, this.transform.position) < Vector3.Distance(closestPoint, this.transform.position))
                {
                    closestPoint = point;
                }
            }

            navAgent.SetDestination(closestPoint);
            Debug.Log(closestPoint);
        }
        
    }

    bool LineOfSightCheck(Vector3 startPosition)
    {
        Vector3 direction = (targetToAvoid.position - startPosition);
        float distance = direction.magnitude;
        direction = direction.normalized;
        Ray ray = new Ray(startPosition, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range) && hit.collider.transform == targetToAvoid)
        {

            return true;
        }
        return false;
    }

    void PoissonGenerator()
    {
        PoissonDiscSampler poissonDisc = new PoissonDiscSampler(poissonDiscWidth, poissonDiscHeight, 3);
        foreach(var point in poissonDisc.Samples())
        {
            Vector3 newPoint = new Vector3(point.x - poissonDiscWidth/2, 0, point.y - poissonDiscHeight/2);
               
            localPoissonPoints.Add(newPoint);
            
          
        }


    }


}
