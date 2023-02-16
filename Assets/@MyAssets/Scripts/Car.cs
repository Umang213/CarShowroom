using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject defaultBody;
    public GameObject body;

    [Header("Points")] public List<Transform> rightSideWheelPoint;
    public List<Transform> leftSideWheelPoint;
    public Transform enginePoint;
    public Transform dBodyPoint;
    public ParticleSystem carColor;
}