using System;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator anim;
    [SerializeField] NavMeshObstacle navMeshObstacle;
    Action _action;
    public Vector3 target;
    public CarPoint carPoint;
    public Car purchaseCar;
    public Transform lastPosition;
    public bool isExit;
    private static readonly int Walk = Animator.StringToHash("Walk");

    /*private void Start()
    {
        noe what do we do next?
        StartCoroutine(EditUpdate());
    }

    IEnumerator EditUpdate()
    {
        yield return new WaitForSeconds(1);
        if (target == transform.position)
        {
            _action?.Invoke();
            Debug.Log("A");
        }

        StartCoroutine(EditUpdate());
    }*/

    public void ExitCustomer()
    {
        ShowSadEmoji();
        navMeshObstacle.enabled = false;
        target = CustomerManager.instance.customerInstantiatePoint.position;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(target);
        anim.SetBool(AnimatorParams.Walk, true);
        CodeMonkey.Utils.FunctionTimer.Create(() => { isExit = true; }, 1);
    }

    public void SetTarget(Vector3 target, Action endTask = null)
    {
        _action = endTask;
        this.target = target;
        navMeshObstacle.enabled = false;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(target);
        anim.SetBool(AnimatorParams.Walk, true);
    }

    public void StopAgent()
    {
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = true;
        anim.SetBool(AnimatorParams.Walk, false);
    }

    public void StopAgentForTask()
    {
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = true;
        anim.SetBool(Walk, false);
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

    public void ShowHappyEmoji()
    {
        var par = CustomerManager.instance.happyEmoji[Helper.RandomInt(0, CustomerManager.instance.happyEmoji.Length)];
        var transform1 = transform;
        var pos = transform1.position;
        pos.y += 3;
        var temp = Instantiate(par, pos, Quaternion.identity, transform1);
        temp.Play();
    }

    private void ShowSadEmoji()
    {
        var par = CustomerManager.instance.sadEmoji[Helper.RandomInt(0, CustomerManager.instance.sadEmoji.Length)];
        var transform1 = transform;
        var pos = transform1.position;
        pos.y += 3;
        var temp = Instantiate(par, pos, Quaternion.identity, transform1);
        temp.Play();
    }
}