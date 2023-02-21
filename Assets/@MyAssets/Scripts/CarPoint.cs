using System;
using System.Collections.Generic;
using UnityEngine;

public class CarPoint : MonoBehaviour
{
    public MoneyStacker moneyStacker;
    public List<Transform> point;
    public Car car;
    public Transform exitPoint;
    public Transform shutter;
    public Vector3 realPos;

    private void Start()
    {
        realPos = transform.parent.position;
    }
}