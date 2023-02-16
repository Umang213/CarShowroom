using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    public Customer Customer;
    public Transform customerInstantiatePoint;

    public List<Customer> allCustomer;
    public List<MyClass> car;

    CarBuildControler _carBuildControler;

    [Serializable]
    public class MyClass
    {
        public List<Transform> point;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        _carBuildControler = CarBuildControler.instance;
        instanceSpawing();
    }

    public void instanceSpawing()
    {
        StartCoroutine(StartSpawing());
    }

    IEnumerator StartSpawing()
    {
        var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        if (count >= 1)
        {
            for (var i = 0; i < count * 3; i++)
            {
                if (allCustomer.Count < count * 3)
                {
                    var temp = Instantiate(Customer, customerInstantiatePoint.position, Quaternion.identity);
                    allCustomer.Add(temp);
                    var carPoint = _carBuildControler.showroomCarPoint[Helper.RandomInt(0, count)];
                    var pos = carPoint.point[Helper.RandomInt(0, 3)];
                    temp.SetTarget(pos.position, (() =>
                    {
                        temp.transform.LookAt(carPoint.transform);
                        SetRandomTarget(temp);
                    }));
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

    public void SetRandomTarget(Customer customer)
    {
        StartCoroutine(GotoNextPoint(customer));
    }

    IEnumerator GotoNextPoint(Customer customer)
    {
        yield return new WaitForSeconds(Helper.RandomInt(5, 10));
        var carPoint = _carBuildControler.showroomCarPoint
            [Helper.RandomInt(0, PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0))];
        var pos = carPoint.point[Helper.RandomInt(0, 3)];
        customer.SetTarget(pos.position,
            (() =>
            {
                customer.transform.LookAt(carPoint.transform);
                FunctionTimer.Create(() =>
                {
                    customer.ExitCustomer();
                    allCustomer.Remove(customer);
                    instanceSpawing();
                }, Helper.RandomInt(5, 10));
            }));
    }
}