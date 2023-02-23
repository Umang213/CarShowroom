using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TriggerEvent : MonoBehaviour
{
    public Image fillImage;
    public UnityEvent buildCarEvent;

    bool _isPlayer;

    private void OnEnable()
    {
        fillImage.fillAmount = 1;
        _isPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            StartCoroutine(BuildCar());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            DOTween.Kill(fillImage);
            DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f);
        }
    }

    IEnumerator BuildCar()
    {
        yield return new WaitForSeconds(0.5f);
        if (_isPlayer)
        {
            DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 0, 1.5f)
                .OnComplete(() => buildCarEvent.Invoke())
                .SetId(fillImage);
        }
    }
}