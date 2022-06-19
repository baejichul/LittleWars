using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventListener : MonoBehaviour
{

    GameManager _gameMgr;
    GameObject _introUI;
    GameObject _playUI;
    GameObject _endUI;
    Transform _controlSet;

    void Start()
    {
        _gameMgr = FindObjectOfType<GameManager>();
        _introUI = _gameMgr.GetGameUIObject("IntroUI");
        _playUI  = _gameMgr.GetGameUIObject("PlayUI");
        _endUI   = _gameMgr.GetGameUIObject("EndUI");
        _controlSet = _playUI.transform.Find("ControlSet");

        SetDifficulty();
        SetCameraScroll();
        SetUnit();
    }

    void SetDifficulty()
    {
        GameObject gObjBtnEasy = _introUI.transform.Find("BtnEasy").gameObject;
        if (gObjBtnEasy is not null)
        {
            Button btnPlay = gObjBtnEasy.GetComponent<Button>();
            btnPlay.onClick.AddListener(() => _gameMgr.PlayGame(DIFFICULTY.EASY));
        }

        GameObject gObjBtnNormal = _introUI.transform.Find("BtnNormal").gameObject;
        if (gObjBtnNormal is not null)
        {
            Button btnPlay = gObjBtnNormal.GetComponent<Button>();
            btnPlay.onClick.AddListener(() => _gameMgr.PlayGame(DIFFICULTY.NORMAL));
        }

        GameObject gObjBtnHard = _introUI.transform.Find("BtnHard").gameObject;
        if (gObjBtnHard is not null)
        {
            Button btnPlay = gObjBtnHard.GetComponent<Button>();
            btnPlay.onClick.AddListener(() => _gameMgr.PlayGame(DIFFICULTY.HARD));
        }
    }

    void SetCameraScroll()
    {
        GameObject gObjBtnRight = _controlSet.Find("BtnRight").gameObject;
        if (gObjBtnRight is not null)
        {
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((eventData) => { _gameMgr.MoveCameraPosX(POINTER.DOWN, DIRECTION.RIGHT); });

            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((eventData) => { _gameMgr.MoveCameraPosX(POINTER.UP, DIRECTION.RIGHT); });

            EventTrigger et = gObjBtnRight.AddComponent<EventTrigger>();
            // gObjBtnRight.GetComponent<EventTrigger>().triggers.Add(entryDown);
            et.triggers.Add(entryDown);
            et.triggers.Add(entryUp);
        }

        GameObject gObjBtnLeft = _controlSet.Find("BtnLeft").gameObject;
        if (gObjBtnLeft is not null)
        {
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((eventData) => { _gameMgr.MoveCameraPosX(POINTER.DOWN, DIRECTION.LEFT); });

            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((eventData) => { _gameMgr.MoveCameraPosX(POINTER.UP, DIRECTION.LEFT); });

            EventTrigger et = gObjBtnLeft.AddComponent<EventTrigger>();
            et.triggers.Add(entryDown);
            et.triggers.Add(entryUp);
        }
    }

    void SetUnit()
    {
        Button[] btnBlueTeam = _controlSet.Find("BlueTeam").GetComponentsInChildren<Button>();
        foreach(Button btn in btnBlueTeam)
        {
            string objNm = "B" + btn.name.Substring(4);
            string classNm = btn.name.Substring(4,1);
            UNIT_CLASS? uc = null;
            if (classNm.Equals("A"))
                uc = UNIT_CLASS.ARCHER;
            else if (classNm.Equals("G"))
                uc = UNIT_CLASS.GUARD;
            else if (classNm.Equals("S"))
                uc = UNIT_CLASS.SWORD;
            else if (classNm.Equals("W"))
                uc = UNIT_CLASS.WIZARD;

            if (objNm.EndsWith("U"))
                btn.onClick.AddListener(() => _gameMgr.UpgradeUnit(objNm, uc.Value));
            else
                btn.onClick.AddListener(() => _gameMgr.BuyUnit(objNm, uc.Value));


        }

        Button[] btnRedTeam  = _controlSet.Find("RedTeam").GetComponentsInChildren<Button>();
        foreach (Button btn in btnRedTeam)
        {
            string objNm = "R" + btn.name.Substring(4);
            string classNm = btn.name.Substring(4, 1);
            UNIT_CLASS? uc = null;
            if (classNm.Equals("A"))
                uc = UNIT_CLASS.ARCHER;
            else if (classNm.Equals("G"))
                uc = UNIT_CLASS.GUARD;
            else if (classNm.Equals("S"))
                uc = UNIT_CLASS.SWORD;
            else if (classNm.Equals("W"))
                uc = UNIT_CLASS.WIZARD;

            if (objNm.EndsWith("U"))
                btn.onClick.AddListener(() => _gameMgr.UpgradeUnit(objNm, uc.Value));
            else
                btn.onClick.AddListener(() => _gameMgr.BuyUnit(objNm, uc.Value));
        }
    }

    

    

    

   
}
