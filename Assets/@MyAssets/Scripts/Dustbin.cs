using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Dustbin : MonoBehaviour
{
    public ParticleSystem smoke;
    public Transform dryDoor;
    public Transform wetDoor;
    public Transform dryPoint;
    public Transform wetPoint;

    bool _isPlayer;
    PlayerController _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            _player = player;
            OpenDoor();
            StartCoroutine(Remove());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isPlayer) return;
        _isPlayer = false;
        CloseDoor();
        StopCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        var count = _player.allStackItems.Count;
        for (var i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            var item = _player.LastFromStack();
            if (item != null && _isPlayer)
            {
                //_player.RemoveFromLast(item, item.isDry ? dryPoint : wetPoint, true);
                var pos = item.isDry ? dryPoint.position : wetPoint.position;
                smoke.transform.position = pos.With(y: pos.y + 0.5f);
                item.transform.SetParent(null);
                item.transform.DOJump(pos, 2, 1, 0.5f).OnComplete(() =>
                {
                    smoke.Play();
                    item.transform.SetParent(null);
                    _player.allStackItems.Remove(item);
                    Destroy(item.gameObject);
                    if (_player.allStackItems.Count == 0)
                    {
                        _player.SetAnimationWeight(1, 0);
                    }
                });
            }
        }
    }

    private readonly Vector3 _rotate = new Vector3(-60, 180, 0);
    private readonly Vector3 _rotate2 = new Vector3(0, 180, 0);

    private void OpenDoor()
    {
        dryDoor.DORotate(_rotate, 1);
        wetDoor.DORotate(_rotate, 1);
    }

    private void CloseDoor()
    {
        dryDoor.DORotate(_rotate2, 1);
        wetDoor.DORotate(_rotate2, 1);
    }
}