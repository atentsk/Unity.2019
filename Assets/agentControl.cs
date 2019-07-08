using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class agentControl : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(0, 0, 0));

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Ray ray_ = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if(Physics.Raycast(ray_, out hitinfo))
            {
                agent.SetDestination(hitinfo.point);
            }
        }

        // Draw Path
        if(agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            line.positionCount = agent.path.corners.Length;
            line.SetPositions(agent.path.corners);
        }
    }
}
