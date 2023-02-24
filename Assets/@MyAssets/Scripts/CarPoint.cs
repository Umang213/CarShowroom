using System.Collections.Generic;
using DG.Tweening;
using EasyButtons;
using UnityEngine;

public class CarPoint : MonoBehaviour
{
    public MoneyStacker moneyStacker;
    public List<Transform> point;
    public Car car;
    public Transform exitPoint;
    public Transform shutter;
    public Vector3 realPos;
    public DOTweenPath path;
    public Transform pathEndPoint;

    private void Start()
    {
        realPos = transform.parent.position;
    }
}