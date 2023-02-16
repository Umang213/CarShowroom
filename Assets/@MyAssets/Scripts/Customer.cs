using System;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator anim;
    [SerializeField] NavMeshObstacle navMeshObstacle;
    CustomerManager _customerManager;
    Action _action;
    public Vector3 target;
    bool _isStop;

    public bool _isExit;

    private void Awake()
    {
        _customerManager = CustomerManager.instance;
    }

    /*private void Start()
    {
        StartCoroutine(EditUpdate());
    }*/

    /*IEnumerator EditUpdate()
    {
        yield return new WaitForSeconds(1);
        if (navMeshAgent.enabled == false && _isStop == true)
        {
            if ((target - transform.position).magnitude > 0)
            {
                SetTarget(target, _action);
            }
        }

        /*if (navMeshAgent.enabled == true)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!_isExit)
                {
                    //StopAgent();
                    StopAgentForTask();
                    _action?.Invoke();
                    _action = null;
                }
            }
        }#1#

        StartCoroutine(EditUpdate());
    }*/

    public void ExitCustomer()
    {
        navMeshObstacle.enabled = false;
        target = CustomerManager.instance.customerInstantiatePoint.position;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(CustomerManager.instance.customerInstantiatePoint.position);
        anim.SetBool("Walk", true);
        CodeMonkey.Utils.FunctionTimer.Create(() => { _isExit = true; }, 1);
    }

    public void SetTarget(Vector3 target, Action endTask = null)
    {
        _action = endTask;
        this.target = target;
        navMeshObstacle.enabled = false;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(target);
        anim.SetBool("Walk", true);
    }

    public void StopAgent()
    {
        _isStop = false;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = true;
        anim.SetBool("Walk", false);
    }

    public void StopAgentForTask()
    {
        _isStop = true;
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = true;
        anim.SetBool("Walk", false);
        _action?.Invoke();
        _action = null;
    }

    public void SetAnimation(String key, bool state)
    {
        anim.SetBool(key, state);
    }

    public void SetAnimation(String key)
    {
        anim.SetTrigger(key);
    }

    /*public void ShowHappyEmoji()
    {
        var par = CustomerManager.instance.happyEmoji[Helper.RandomInt(0, CustomerManager.instance.happyEmoji.Length)];
        var pos = transform.position;
        pos.y += 3;
        var temp = Instantiate(par.gameObject, pos, Quaternion.identity, transform);
        temp.GetComponent<ParticleSystem>().Play();
    }*/

    /*public void ShowSadEmoji()
    {
        var par = CustomerManager.instance.sadEmoji[Helper.RandomInt(0, CustomerManager.instance.sadEmoji.Length)];
        var pos = transform.position;
        pos.y += 3;
        var temp = Instantiate(par.gameObject, pos, Quaternion.identity, transform);
        temp.GetComponent<ParticleSystem>().Play();
    }*/
}