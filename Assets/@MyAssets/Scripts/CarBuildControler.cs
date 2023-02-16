using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EasyButtons;
using UnityEngine;

public class CarBuildControler : MonoBehaviour
{
    public static CarBuildControler instance;
    public ParticleSystem smoke;
    public List<Car> allCars;
    public Transform[] processPoint;
    public MeshRenderer conveyorBelt;

    [Header("AddWheel")] public RoboHand roboHand;
    public RoboHand roboHand2;
    public List<WheelPoint> WheelPoints;
    public int fixTyreCount;

    [Header("AddEngine")] public RoboHand engineRoboHand;
    public WheelPoint enginePoint;
    public int fixEngineCount;

    [Header("AddColor")] public Animator colorRoboHand;
    public Transform colorPoint;

    [Header("AddBody")] public RoboHand dBodyRoboHand;
    public Transform dBodyPoint;

    [Header("Collectable")] public Collectables wheel;
    public Collectables engine;
    public Collectables color;

    [Header("Add Trigger")] public GameObject buildCar;
    public GameObject addWheel;
    public GameObject addEngine;
    public GameObject addBody;
    public GameObject addColor;

    [Header("Show_Room_Point")] public CarPoint[] showroomCarPoint;

    PlayerController _player;

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

        LoadData();
    }

    private void Start()
    {
        _player = PlayerController.instance;
    }

    [Button]
    public void BuildCar()
    {
        var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var car = allCars[count];
        buildCar.Hide();
        car.Show();
        car.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack).OnComplete(() =>
        {
            conveyorBelt.materials[1].DOOffset(new Vector2(0, 1f), 2).SetEase(Ease.Linear).From(Vector2.zero);
            conveyorBelt.materials[1].DOOffset(new Vector2(0, 1f), "_BumpMap", 2).SetEase(Ease.Linear)
                .From(Vector2.zero);
            allCars[count].transform.DOMove(processPoint[0].position, 2).SetEase(Ease.Linear)
                .OnComplete(addWheel.Show);
        });
    }

    public void AddWheel()
    {
        for (var i = 0; i < 4; i++)
        {
            if (WheelPoints[i].wheel == null)
            {
                var pos = WheelPoints[i].stackPoint;
                var temp = _player.RemoveFromLast(wheel, pos);
                if (temp != null)
                {
                    WheelPoints[i].wheel = temp;
                }
            }
        }

        CodeMonkey.Utils.FunctionTimer.Create(() =>
        {
            roboHand.AddTyre(this);
            roboHand2.AddTyre(this);
        }, 1);
    }

    public void CheckTyreIsFix()
    {
        if (fixTyreCount.Equals(4))
        {
            addWheel.Hide();
            NextPoint(2, addEngine);
            fixTyreCount = 0;
        }
    }

    public void AddEngine()
    {
        if (enginePoint.wheel == null)
        {
            var pos = enginePoint.stackPoint;
            var temp = _player.RemoveFromLast(engine, pos);
            if (temp != null)
            {
                enginePoint.wheel = temp;
            }
        }

        CodeMonkey.Utils.FunctionTimer.Create(() => { engineRoboHand.AddEngine(this); }, 1);
    }

    public void CheckEngineIsFix()
    {
        if (fixEngineCount.Equals(1))
        {
            addEngine.Hide();
            NextPoint(3, addBody);
            fixEngineCount = 0;
        }
    }

    public void AddBody()
    {
        var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var car = allCars[count].defaultBody;
        car.transform.position = dBodyPoint.position;
        car.transform.rotation = dBodyPoint.rotation;

        car.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack).OnComplete(() =>
        {
            addBody.Hide();
            dBodyRoboHand.AddBody(this);
        });
    }

    public void AfterAddingBody()
    {
        NextPoint(4, addColor);
    }

    public void AddColor()
    {
        StartCoroutine(StartAddColor());
    }

    IEnumerator StartAddColor()
    {
        var player = PlayerController.instance;
        var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var body = allCars[count].body;
        var temp = player.RemoveAll(color);
        if (temp.Any())
        {
            smoke.transform.position = body.transform.position.With(y: body.transform.position.y - 0.5f);
            colorRoboHand.SetBool("Color", true);
            allCars[count].carColor.Show();
            allCars[count].carColor.Play();
            for (var i = 0; i < temp.Count; i++)
            {
                var i1 = i;
                temp[i1].transform.DOJump(body.transform.position, 2, 1, 0.5f)
                    .OnComplete(() =>
                    {
                        smoke.Play();
                        temp[i1].transform.SetParent(body.transform);
                        temp[i1].gameObject.Hide();
                    });
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(1.5f);
            allCars[count].defaultBody.Hide();
            body.Show();
            smoke.Stop();
            addColor.Hide();
            yield return new WaitForSeconds(2f);
            colorRoboHand.SetBool("Color", false);
            allCars[count].carColor.Stop();
            allCars[count].carColor.Hide();
            AddInShowRoom();
        }
    }

    private void NextPoint(int point, GameObject showGameObject)
    {
        CodeMonkey.Utils.FunctionTimer.Create(() =>
        {
            var temp = point * 1.5f;
            conveyorBelt.materials[1].DOOffset(new Vector2(0, temp), 3).SetEase(Ease.Linear);
            conveyorBelt.materials[1].DOOffset(new Vector2(0, temp), "_BumpMap", 2).SetEase(Ease.Linear);
            allCars[PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0)].transform
                .DOMove(processPoint[point - 1].position, 3f)
                .SetEase(Ease.Linear).OnComplete(showGameObject.Show);
        }, 2);
    }

    private void AddInShowRoom()
    {
        CodeMonkey.Utils.FunctionTimer.Create(() =>
        {
            var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
            var car = allCars[count];
            car.transform.DOScale(Vector3.zero, 1.8f).SetDelay(1);
            car.transform.DOMove(processPoint[4].position, 2).SetEase(Ease.Linear).OnComplete(() =>
            {
                var transform1 = car.transform;
                transform1.position = showroomCarPoint[count].transform.position;
                transform1.rotation = showroomCarPoint[count].transform.rotation;
                car.transform.SetParent(showroomCarPoint[count].transform);
                transform1.DOScale(Vector3.one, 1).SetEase(Ease.Linear);
                showroomCarPoint[count].transform.DORotate(new Vector3(0, 180, 0), 10, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                CustomerManager.instance.instanceSpawing();
            });
            PlayerPrefs.SetInt(PlayerPrefsKey.CarBuildIndex, count + 1);
            buildCar.Show();
        }, 2);
    }

    private void LoadData()
    {
        var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        for (var i = 0; i < count; i++)
        {
            allCars[i].transform.localScale = Vector3.one;
            var temp = allCars[i].rightSideWheelPoint[0];
            var wheel1 = Instantiate(wheel, temp.position, temp.rotation);
            wheel1.transform.SetParent(temp);
            var temp2 = allCars[i].rightSideWheelPoint[1];
            var wheel2 = Instantiate(wheel, temp2.position, temp2.rotation);
            wheel2.transform.SetParent(temp);
            var temp3 = allCars[i].leftSideWheelPoint[0];
            var wheel3 = Instantiate(wheel, temp3.position, temp3.rotation);
            wheel3.transform.SetParent(temp);
            var temp4 = allCars[i].leftSideWheelPoint[1];
            var wheel4 = Instantiate(wheel, temp4.position, temp4.rotation);
            wheel4.transform.SetParent(temp);
            allCars[i].body.Show();
            allCars[i].transform.position = showroomCarPoint[i].transform.position;
            allCars[i].transform.rotation = showroomCarPoint[i].transform.rotation;
            allCars[i].transform.SetParent(showroomCarPoint[i].transform);
            showroomCarPoint[i].transform.DORotate(new Vector3(0, 180, 0), 10, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }
    }
}