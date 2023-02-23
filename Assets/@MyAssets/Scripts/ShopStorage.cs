using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShopStorage : MonoBehaviour
{
    public Transform[] points;
    public Collectables prefab;
    PlayerController _playerController;
    bool _isPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _playerController = player;
            _isPlayer = true;
            StartCoroutine(AddToPlayerStack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
        }
    }

    IEnumerator AddToPlayerStack()
    {
        var count = _playerController.maxStackCount - _playerController.allStackItems.Count;
        for (var i = 0; i < count; i++)
        {
            if (_isPlayer)
            {
                //if (_playerController.IsStackFull()) yield break;
                if (_playerController.allStackItems.Count.Equals(_playerController.maxStackCount)) yield break;
                var pos = points[Helper.RandomInt(0, points.Length)];
                pos.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                {
                    var temp = Instantiate(prefab, pos.position, pos.rotation);
                    _playerController.AddToStack(temp);
                    pos.DOScale(Vector3.one, 0.2f).SetDelay(0.2f);
                });
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}