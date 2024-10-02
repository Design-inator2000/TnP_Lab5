using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Avoider : MonoBehaviour
{
    [SerializeField]
    bool showGizmo = true;
    [SerializeField]
    Transform targetToAvoid;
    [SerializeField]
    float distanceWhenToAvoid;
    [SerializeField]
    float poissonDiscHeight, poissonDiscWidth;
    List<Vector3> localPoissonPoints=new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        PoissonGenerator();

    }

    // Update is called once per frame
    void Update()
    {

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
                    }
                    else
                    {
                        Debug.DrawLine(this.transform.position, this.transform.TransformPoint(point), Color.red);
                    }

                }
            }


        }
        
    }

    bool LineOfSightCheck(Vector3 startPosition)
    {
        Vector3 direction = (targetToAvoid.position - startPosition);
        float distance = direction.magnitude;
        direction = direction.normalized;
        Ray ray = new Ray(startPosition, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distanceWhenToAvoid) && hit.collider.transform == targetToAvoid)
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
