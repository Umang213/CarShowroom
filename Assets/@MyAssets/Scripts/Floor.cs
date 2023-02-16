using UnityEngine;

public class Floor : MonoBehaviour
{
    private bool _isPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            player.PlayerInShowroom();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            player.PlayerInGarage();
        }
    }
}