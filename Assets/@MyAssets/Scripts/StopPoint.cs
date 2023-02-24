using UnityEngine;

public class StopPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            if (customer.isExit)
            {
                if (customer.target != transform.position) return;
                customer.StopAgent();
                Destroy(customer.gameObject);
            }
            else
            {
                if (customer.target == transform.position)
                {
                    customer.StopAgentForTask();
                }
            }
        }

        if (other.CompareTag("Car"))
        {
            Destroy(other.gameObject);
        }
    }
}