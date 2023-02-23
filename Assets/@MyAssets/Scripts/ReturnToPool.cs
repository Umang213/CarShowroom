using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour
{
    public Collectables item;
    public IObjectPool<Collectables> pool;

    void Start()
    {
        item = GetComponent<Collectables>();
        //var main = item.main;
        //main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        // Return to the pool
        pool.Release(item);
    }
}