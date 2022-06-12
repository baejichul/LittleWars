using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool _isClearEasy;
    public bool _isClearNomal;
    public bool _isClearHard;

    public ConfigManager _cfgMgr;
    public GameObject _introUI;
    public GameObject _playUI;
    public GameObject _endUI;

    public UIMode _uiMode;
    public Difficulty _difficulty;
    private float titleSetVal = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _cfgMgr = FindObjectOfType<ConfigManager>();
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {   
        moveTitleSet();
    }    

    void InitGame()
    {
        InitGameUIObject();
        if (_introUI != null)
        {
            _uiMode = UIMode.INTRO;
            setActiveClearImage();
        }
    }

    public void InitGameUIObject()
    {
        _introUI = GameObject.FindWithTag("IntroUI");
        _playUI  = GameObject.FindWithTag("PlayUI");
        _endUI   = GameObject.FindWithTag("EndUI");
    }

    public void playGame()
    {
        if (_playUI != null)
        {
            _uiMode = UIMode.PLAY;
            _difficulty = Difficulty.EASY;
        }
    }

    public void playGame(Difficulty df)
    {
        if (_playUI != null)
        {
            _uiMode = UIMode.PLAY;
            _difficulty = df;
            //Debug.Log(_uiMode.ToString() + " : " + df.ToString());
        }
    }

    public void endGame()
    {
        if (_endUI != null)
        {
            _uiMode = UIMode.END;
        }
    }

    public GameObject getGameUIObject(string uiName)
    {
        return GameObject.FindWithTag(uiName);
    }

    // 난이도 클리어 설정
    void setActiveClearImage()
    {
        if (_uiMode == UIMode.INTRO)
        {
            Transform btnEasyTf = _introUI.transform.Find("ButtonEasy");
            if (btnEasyTf != null)
                btnEasyTf.gameObject.transform.Find("Image").gameObject.SetActive(_isClearEasy);

            Transform btnNormalTf = _introUI.transform.Find("ButtonNormal");
            if (btnNormalTf != null)
                btnNormalTf.gameObject.transform.Find("Image").gameObject.SetActive(_isClearNomal);

            Transform btnHardTf = _introUI.transform.Find("ButtonHard");
            if (btnHardTf != null)
                btnHardTf.gameObject.transform.Find("Image").gameObject.SetActive(_isClearHard);
        }
    }

    // 타이틀셋 애니메이션
    void moveTitleSet()
    {
        if (_uiMode == UIMode.INTRO)
        {
            titleSetVal = titleSetVal + 0.015f;
            // Debug.Log("val = " + titleSetVal + " : Mathf.Sin(val) = " + Mathf.Sin(titleSetVal));
            Transform titleTf = _introUI.transform.Find("TitleSet");
            if (titleTf != null)
                titleTf.Translate(new Vector3(0.0f, Mathf.Sin(titleSetVal) * 0.15f, 0.0f));
        }
    }


    
}
