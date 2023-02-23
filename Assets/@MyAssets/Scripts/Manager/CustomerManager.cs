using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    [FormerlySerializedAs("Customer")] public List<Customer> customers;
    public Transform customerInstantiatePoint;

    public List<Customer> allCustomer;

    public ParticleSystem[] happyEmoji;
    public ParticleSystem[] sadEmoji;
    public ParticleSystem ConfettiBlast;


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
        //var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var points = _carBuildControler.showroomCarPoint.FindAll(x => x.car != null);
        if (points.Count >= 1)
        {
            for (var i = 0; i < points.Count * 2; i++)
            {
                if (allCustomer.Count < points.Count * 2)
                {
                    var temp = Instantiate(customers[Helper.RandomInt(0, customers.Count)],
                        customerInstantiatePoint.position, Quaternion.identity);
                    allCustomer.Add(temp);
                    var carPoint = points[Helper.RandomInt(0, points.Count)];
                    var pos = carPoint.point[Helper.RandomInt(0, 3)];
                    temp.lastPosition = pos;
                    temp.carPoint = carPoint;
                    temp.SetTarget(pos.position, (() =>
                    {
                        if (temp.carPoint.car == null)
                        {
                            temp.ExitCustomer();
                            allCustomer.Remove(temp);
                        }
                        else
                        {
                            temp.transform.LookAt(carPoint.transform);
                            SetRandomTarget(temp);
                        }
                    }));
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

    private void SetRandomTarget(Customer customer)
    {
        StartCoroutine(GotoNextPoint(customer));
    }

    IEnumerator GotoNextPoint(Customer customer)
    {
        yield return new WaitForSeconds(Helper.RandomInt(5, 10));
        var allCarPoints = _carBuildControler.showroomCarPoint.FindAll(x => x.car != null);
        if (allCarPoints.Any())
        {
            var carPoint = allCarPoints[Helper.RandomInt(0, allCarPoints.Count)];
            //var pos = carPoint.point[Helper.RandomInt(0, 3)];
            Transform pos = null;
            if (customer.lastPosition == carPoint.point[0])
            {
                pos = carPoint.point[1];
            }
            else if (customer.lastPosition == carPoint.point[1])
            {
                pos = carPoint.point[0];
            }
            else
            {
                pos = carPoint.point[0];
            }

            customer.carPoint = carPoint;
            customer.SetTarget(pos.position,
                (() =>
                {
                    customer.transform.LookAt(carPoint.transform);
                    FunctionTimer.Create(() =>
                    {
                        if (customer.carPoint.car == null)
                        {
                            customer.ExitCustomer();
                        }
                        else
                        {
                            var count = Random.Range(0, 10);
                            if (count < 7)
                            {
                                _carBuildControler.PurchaseCar(customer);
                            }
                            else
                            {
                                customer.ExitCustomer();
                            }
                        }

                        allCustomer.Remove(customer);
                        instanceSpawing();
                    }, Helper.RandomInt(5, 10));
                }));
        }
        else
        {
            customer.ExitCustomer();
            allCustomer.Remove(customer);
        }
    }

    public void CheckForExit()
    {
        for (var i = 0; i < allCustomer.Count; i++)
        {
            if (allCustomer[i].carPoint.car == null)
            {
                allCustomer[i].ExitCustomer();
                allCustomer.Remove(allCustomer[i]);
            }
        }
    }
}