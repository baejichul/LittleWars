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

    void Start()
    {
        _gameMgr = FindObjectOfType<GameManager>();
        _introUI = _gameMgr.GetGameUIObject("IntroUI");
        _playUI = _gameMgr.GetGameUIObject("PlayUI");
        _endUI = _gameMgr.GetGameUIObject("EndUI");

        SetDifficulty();
        SetCameraScroll();
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
        GameObject gObjBtnRight = _playUI.transform.Find("ControlSet").Find("BtnRight").gameObject;
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

        GameObject gObjBtnLeft = _playUI.transform.Find("ControlSet").Find("BtnLeft").gameObject;
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

    

    

    

    

   
}
