using UnityEngine;

public class DestroyAllChild : MonoBehaviour
{
    public void RemoveChild()
    {
        var childs = GetComponentsInChildren<Transform>();
        foreach (var t in childs)
        {
            Destroy(t.gameObject);
        }
    }
}