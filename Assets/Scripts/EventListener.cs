using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventListener : MonoBehaviour
{

    GameManager _gameMgr;
    GameObject _introUI;

    void Start()
    {

        _gameMgr = FindObjectOfType<GameManager>();
        _gameMgr.InitGameUIObject();
        _introUI = _gameMgr._introUI;

        // 버튼 이벤트 리스너 등록
        GameObject gObjBtnEasy = _introUI.transform.Find("BtnEasy").gameObject;
        if (gObjBtnEasy is not null)
        {
            Button btnPlay = gObjBtnEasy.GetComponent<Button>();
            btnPlay.onClick.AddListener(() => PlayGame(Difficulty.EASY));
        }

        GameObject gObjBtnNormal = _introUI.transform.Find("BtnNormal").gameObject;
        if (gObjBtnNormal is not null)
        {
            Button btnPlay = gObjBtnNormal.GetComponent<Button>();
            btnPlay.onClick.AddListener(() => PlayGame(Difficulty.NORMAL));
        }

        GameObject gObjBtnHard = _introUI.transform.Find("BtnHard").gameObject;
        if (gObjBtnHard is not null)
        {
            Button btnPlay = gObjBtnHard.GetComponent<Button>();
            btnPlay.onClick.AddListener(() => PlayGame(Difficulty.HARD));
        }


    }

    void PlayGame(Difficulty df)
    {
        _gameMgr.playGame(df);
    }

    

   
}
