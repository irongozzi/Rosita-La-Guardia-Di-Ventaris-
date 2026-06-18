using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class MinerEnemyBehaviour : MonoBehaviour
{
    public float Range;

    NavMeshAgent enemyAgent;
    [SerializeField] float timer;
    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        timer = Random.Range(3, 8);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            //timer per attendere prima di ricevere una nuova posizione
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                //se è stato trovato un punto, lo mette come Destination dell'agent e riassegna timer, altrimenti rigenera punto
                Vector3 point;
                if (RandomPoint(transform.position, Range, out point))
                {
                    enemyAgent.SetDestination(point);
                    timer = Random.Range(3, 8);
                }
            }
        }
    }

    #region METODI&FUNZ

    //funzione per ottenere un nuovo punto casuale

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //crea un punto casuale in una sfera con raggio range
        Vector3 randomPoint = center + (Random.insideUnitSphere * range);

        NavMeshHit hit;
        //se il punto non è sulla NavMeshSurface, cerca il punto più vicino sulla surface
        if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    
    #endregion
}
