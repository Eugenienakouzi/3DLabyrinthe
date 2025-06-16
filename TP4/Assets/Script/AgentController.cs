using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform cible;

    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(cible.position);
    }
}
