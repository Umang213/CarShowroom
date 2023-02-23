using UnityEngine;

public class RoboHand : MonoBehaviour
{
    [SerializeField] Animator _anim;
    public Transform pickPoint;

    [Header("AddWheel")] [SerializeField] bool isRightSide;

    CarBuildControler _carBuildControler;

    WheelPoint _rightPoint;
    WheelPoint _leftPoint;

    public void AddTyre(CarBuildControler carBuildControler)
    {
        _carBuildControler = carBuildControler;
        if (isRightSide)
        {
            _rightPoint = carBuildControler.WheelPoints[0];
            _leftPoint = carBuildControler.WheelPoints[1];
        }
        else
        {
            _rightPoint = carBuildControler.WheelPoints[2];
            _leftPoint = carBuildControler.WheelPoints[3];
        }

        CheckPossibility();
    }

    public void CheckPossibility()
    {
        if (_rightPoint.wheel != null)
        {
            _anim.SetBool("Right", true);
        }
        else if (_leftPoint.wheel != null)
        {
            _anim.SetBool("Left", true);
        }
        else
        {
            _carBuildControler.CheckTyreIsFix();
        }
    }

    public void PickRightTyre()
    {
        _rightPoint.wheel.transform.position = pickPoint.position;
        _rightPoint.wheel.transform.SetParent(pickPoint);
    }

    public void PickLeftTyre()
    {
        _leftPoint.wheel.transform.position = pickPoint.position;
        _leftPoint.wheel.transform.SetParent(pickPoint);
    }

    public void AddRightTyre()
    {
        //  var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var temp = isRightSide
            ? _carBuildControler.currentCar.rightSideWheelPoint[0]
            : _carBuildControler.currentCar.leftSideWheelPoint[0];

        _rightPoint.wheel.transform.position = temp.position;
        _rightPoint.wheel.transform.rotation = temp.rotation;
        _rightPoint.wheel.transform.SetParent(temp);
        _rightPoint.wheel = null;
        _carBuildControler.fixTyreCount += 1;
        _anim.SetBool("Right", false);
    }

    public void AddLeftTyre()
    {
        //var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var temp = isRightSide
            ? _carBuildControler.currentCar.rightSideWheelPoint[1]
            : _carBuildControler.currentCar.leftSideWheelPoint[1];
        _leftPoint.wheel.transform.position = temp.position;
        _leftPoint.wheel.transform.rotation = temp.rotation;
        _leftPoint.wheel.transform.SetParent(temp);
        _leftPoint.wheel = null;
        _carBuildControler.fixTyreCount += 1;
        _anim.SetBool("Left", false);
    }

    WheelPoint _enginePoint;

    public void AddEngine(CarBuildControler carBuildControler)
    {
        _carBuildControler = carBuildControler;
        _enginePoint = carBuildControler.enginePoint;
        if (_enginePoint.wheel != null)
        {
            _anim.SetBool("Engine", true);
        }
    }

    public void PickEngine()
    {
        _enginePoint.wheel.transform.position = pickPoint.position;
        _enginePoint.wheel.transform.SetParent(pickPoint);
    }

    public void FitEngine()
    {
        //var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var temp = _carBuildControler.currentCar.enginePoint;
        _enginePoint.wheel.transform.position = temp.position;
        _enginePoint.wheel.transform.rotation = temp.rotation;
        _enginePoint.wheel.transform.SetParent(temp);
        _enginePoint.wheel = null;
        _carBuildControler.fixEngineCount += 1;
        _anim.SetBool("Engine", false);
        _carBuildControler.CheckEngineIsFix();
    }

    public void AddBody(CarBuildControler carBuildControler)
    {
        _carBuildControler = carBuildControler;
        _anim.SetBool("Body", true);
        //make customer entry , customer random walking in showroom , add Gate , Check obi cloth Package
    }

    public void PickBody()
    {
        //var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        _carBuildControler.currentCar.defaultBody.transform.position = pickPoint.position;
        _carBuildControler.currentCar.defaultBody.transform.SetParent(pickPoint);
    }

    public void FitBody()
    {
        //var count = PlayerPrefs.GetInt(PlayerPrefsKey.CarBuildIndex, 0);
        var dbody = _carBuildControler.currentCar.defaultBody;
        dbody.transform.position = _carBuildControler.currentCar.dBodyPoint.position;
        dbody.transform.rotation = _carBuildControler.currentCar.dBodyPoint.rotation;
        dbody.transform.SetParent(_carBuildControler.currentCar.dBodyPoint);
        _anim.SetBool("Body", false);
        _carBuildControler.AfterAddingBody();
    }
}