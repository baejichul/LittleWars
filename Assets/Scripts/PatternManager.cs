using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PatternManager : MonoBehaviour
{
    DIFFICULTY _diff;
    UNIT_CLASS _unitClass;

    GameManager _gMgr;
    string gObjNm;
    float _duration = 0.0f;
    int _cnt  = 0;
    int _unitCnt = 0;

    public void Init(DIFFICULTY df)
    {
        _gMgr = FindObjectOfType<GameManager>();
        _diff = df;

        if (_diff == DIFFICULTY.EASY)
        {
            _duration = 1.0f;   //6.0f
            _unitCnt = 10;
        }
        else if (_diff == DIFFICULTY.NORMAL)
        {
            _duration = 5.0f;
            _unitCnt = 20;
        }
        else if (_diff == DIFFICULTY.HARD)
        {
            _duration = 1.0f;   //4.0f
            _unitCnt = 50;
        }

        StartCoroutine(SetPattern());
    }

    public IEnumerator SetPattern()
    {
        if (_cnt < _unitCnt && _gMgr._uiMode == UIMODE.PLAY)
        {
            _cnt++;
            yield return new WaitForSeconds(_duration);
            CreatePattern();
        }
    }

    void CreatePattern()
    {
        if (_diff == DIFFICULTY.EASY)
        {
            int rnClass = Random.Range(0, 4);
            int isBuy = Random.Range(0, 4);
            _unitClass = GetUnitClass(rnClass);
            gObjNm = GetGameObjNm(_unitClass, isBuy);
            _gMgr.BuyUnit(gObjNm, _unitClass);
        }
        else if (_diff == DIFFICULTY.NORMAL)
        {
            int rnClass = Random.Range(0, 4);
            int isBuy = Random.Range(0, 3);
            _unitClass = GetUnitClass(rnClass);
            gObjNm = GetGameObjNm(_unitClass, isBuy);

            if (isBuy == 0)
                _gMgr.UpgradeUnit(gObjNm, _unitClass);
            else
                _gMgr.BuyUnit(gObjNm, _unitClass);
        }
        else if (_diff == DIFFICULTY.HARD)
        {
            int rnClass = Random.Range(0, 4);
            int isBuy = Random.Range(0, 2);
            _unitClass = GetUnitClass(rnClass);
            gObjNm = GetGameObjNm(_unitClass, isBuy);

            if (isBuy == 0)
                _gMgr.UpgradeUnit(gObjNm, _unitClass);
            else
                _gMgr.BuyUnit(gObjNm, _unitClass);
        }

        StopCoroutine(SetPattern());
        StartCoroutine(SetPattern());
    }

    UNIT_CLASS GetUnitClass(int val)
    {
        UNIT_CLASS? rtn = null;
        if (val == 0) rtn = UNIT_CLASS.SWORD;
        else if (val == 1) rtn = UNIT_CLASS.GUARD;
        else if (val == 2) rtn = UNIT_CLASS.ARCHER;
        else if (val == 3) rtn = UNIT_CLASS.WIZARD;
        return rtn.Value;
    }

    string GetGameObjNm(UNIT_CLASS uc, int isBuy)
    {
        string rtn = "R";
        if (uc == UNIT_CLASS.SWORD) rtn += "S";
        else if (uc == UNIT_CLASS.GUARD) rtn += "G";
        else if (uc == UNIT_CLASS.ARCHER) rtn += "A";
        else if (uc == UNIT_CLASS.WIZARD) rtn += "W";

        if (isBuy == 0) rtn += "U";

        return rtn;
    }



}
