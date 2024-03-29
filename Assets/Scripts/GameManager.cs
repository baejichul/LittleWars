using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool _isClearEasy  = false;
    public static bool _isClearNomal = false;
    public static bool _isClearHard  = false;
    public static bool _isVictory  = false;

    public ConfigManager _cfgMgr;
    public SoundManager _sndMgr;
    public PatternManager _ptnMgr;

    GameObject _canvas;
    GameObject _baseGroup;
    GameObject _backGroup;

    public GameObject _introUI;
    public GameObject _playUI;
    public GameObject _endUI;

    public GameObject _camera;
    GameObject _targetObj;

    public UIMODE _uiMode;
    public DIFFICULTY _difficulty;
    Transform imgCost;

    float _titleSetVal = 0.0f;
    int _cameraPosX = 0;
    bool _isCarmeraMove = true;
    public int _cost = 0;
    int _maxCost = 10;
    float _maxBarwidth;

    // Start is called before the first frame update
    void Start()
    {
        _cfgMgr = FindObjectOfType<ConfigManager>();
        _sndMgr = FindObjectOfType<SoundManager>();
        _ptnMgr = FindObjectOfType<PatternManager>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera");

        InitGameUIObject();
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        if ( _uiMode == UIMODE.INTRO )
            MoveTitleSet("GameTitle");
        else if (_uiMode == UIMODE.END && _isVictory == true)
            MoveTitleSet("GameVictory");
        else if (_uiMode == UIMODE.END && _isVictory == false)
            MoveTitleSet("GameDefeat");

        MoveCamera();
        SetBlueMenu();
    }

    private void LateUpdate()
    {
        MoveBackground();
    }
    
    public void setVictory(bool val)
    {
        _isVictory = val;
    }

    public void InitGame()
    {   
        if (_introUI != null)
        {   
            _uiMode = UIMODE.INTRO;

            _introUI.SetActive(true);
            _playUI.SetActive(false);
            _endUI.SetActive(false);

            setActiveBase(false);
            SetActiveClearImage();

            _sndMgr.Play("BGM");

            // 유닛 구매 가격설정
            _cfgMgr._cost[UNIT_CLASS.SWORD] = 3;
            _cfgMgr._cost[UNIT_CLASS.GUARD] = 3;
            _cfgMgr._cost[UNIT_CLASS.WIZARD] = 4;
            _cfgMgr._cost[UNIT_CLASS.ARCHER] = 5;
        }
    }

    public void InitGameUIObject()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _introUI = _canvas.transform.Find("IntroUI").gameObject;
        _playUI  = _canvas.transform.Find("PlayUI").gameObject;
        _endUI   = _canvas.transform.Find("EndUI").gameObject;
    }

    public void PlayGame()
    {
        if (_playUI != null)
            PlayGame(DIFFICULTY.EASY);
    }

    public void PlayGame(DIFFICULTY df)
    {
        if (_playUI != null)
        {
            _isVictory = false;
            _uiMode = UIMODE.PLAY;
            _difficulty = df;
            
            _introUI.SetActive(false);
            _playUI.SetActive(true);
            _endUI.SetActive(false);

            setActiveBase(true);
            Invoke("IncreaseCostNow", 2.0f);

            InitCostBar();
            InitEnemyAI(df);
        }
    }

    // 2초 간격으로 Cost 증가
    void IncreaseCostNow()
    {
        if (_cost < _maxCost)
        {
            _cost = _cost + 1;
            // SetCostBar();
        }
        Invoke("IncreaseCostNow", 2.0f);
    }

    void InitCostBar()
    {
        imgCost = _playUI.transform.Find("ControlSet").transform.Find("ImgCost");
        if (imgCost is not null)
        {
            Vector2 vec = imgCost.Find("Bar").GetComponent<RectTransform>().sizeDelta;
            _maxBarwidth = vec.x;               // 최대값 저장

            imgCost.Find("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, vec.y);      // 0으로 초기화
        }
    }

    void InitEnemyAI(DIFFICULTY df)
    {
        _ptnMgr.Init(df);
    }

    void SetBlueMenu()
    {
        SetUpgradeBtn();
        SetCostBar();
    }

    void SetUpgradeBtn()
    {
        if (_uiMode == UIMODE.PLAY)
        {
            GameObject btObj = _playUI.transform.Find("ControlSet").transform.Find("BlueTeam").gameObject;
            if (btObj is not null)
            {
                if (_cost >= _cfgMgr._cost[UNIT_CLASS.ARCHER] * _cfgMgr._upgradeCost)
                {
                    btObj.transform.Find("BtnBAU").GetComponent<Image>().enabled = true;
                    btObj.transform.Find("BtnBAU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = true;
                }
                else
                {
                    btObj.transform.Find("BtnBAU").GetComponent<Image>().enabled = false;
                    btObj.transform.Find("BtnBAU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
                }

                if (_cost >= _cfgMgr._cost[UNIT_CLASS.GUARD] * _cfgMgr._upgradeCost)
                {
                    btObj.transform.Find("BtnBGU").GetComponent<Image>().enabled = true;
                    btObj.transform.Find("BtnBGU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = true;
                }
                else
                {
                    btObj.transform.Find("BtnBGU").GetComponent<Image>().enabled = false;
                    btObj.transform.Find("BtnBGU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
                }

                if (_cost >= _cfgMgr._cost[UNIT_CLASS.SWORD] * _cfgMgr._upgradeCost)
                {
                    btObj.transform.Find("BtnBSU").GetComponent<Image>().enabled = true;
                    btObj.transform.Find("BtnBSU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = true;
                }
                else
                {
                    btObj.transform.Find("BtnBSU").GetComponent<Image>().enabled = false;
                    btObj.transform.Find("BtnBSU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
                }

                if (_cost >= _cfgMgr._cost[UNIT_CLASS.WIZARD] * _cfgMgr._upgradeCost)
                {
                    btObj.transform.Find("BtnBWU").GetComponent<Image>().enabled = true;
                    btObj.transform.Find("BtnBWU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = true;
                }
                else
                {
                    btObj.transform.Find("BtnBWU").GetComponent<Image>().enabled = false;
                    btObj.transform.Find("BtnBWU").gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
                }
            }
        }
    }
       

    void SetCostBar()
    {
        if (_uiMode == UIMODE.PLAY)
        {
            float bar = _maxBarwidth * (float)_cost / (float)_maxCost;
            Vector2 vec = imgCost.Find("Bar").GetComponent<RectTransform>().sizeDelta;

            if (imgCost is not null)
            {
                imgCost.Find("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(bar, vec.y);
                imgCost.Find("TxtShadow").GetComponent<Text>().text = _cost.ToString("D2");
                imgCost.Find("TxtCost").GetComponent<Text>().text = _cost.ToString("D2");
            }
        }
    }

    void SetPortrait(string gameObjNm, UNIT_CLASS uc)
    {
        // 이미지 설정
        Sprite sp = null;
        string resourceNm = "Portrait " + gameObjNm.Substring(1);
        Sprite[] spArr = Resources.LoadAll<Sprite>(_cfgMgr.defaultSpritesUIPath);
        foreach (Sprite sprite in spArr)
        {
            if (sprite.name.Equals(resourceNm))
            {
                sp = sprite;
                break;
            }
        }

        // HP, POWER, COST 설정
        Unit ut = _targetObj.GetComponent<Unit>();
        int level = ut._unitConfig._level;
        int unitCs = _cfgMgr._cost[uc];
        if (gameObjNm.EndsWith("U"))
        {
            gameObjNm = gameObjNm.Substring(0, 2);
            unitCs = unitCs * _cfgMgr._upgradeCost;
            level = level + 1;
        }
        
        ut.InitUnitConfig(level);
        int unitHp = ut._unitConfig._hp;
        int unitPw = ut._unitConfig._power;
        int unitLv = ut._unitConfig._level;

        if (gameObjNm.EndsWith("U"))
            gameObjNm = gameObjNm.Substring(0, 2);

        GameObject gObj = _playUI.transform.Find("ControlSet").transform.Find("BlueTeam").transform.Find("Btn" + gameObjNm).gameObject;
        if (gObj is not null)
        {
            if (sp is not null)
                gObj.transform.Find("Image").GetComponent<Image>().sprite = sp;

            gObj.transform.Find("HP").transform.Find("Text").GetComponent<Text>().text = string.Format("{0}K", (double)unitHp / 1000);
            gObj.transform.Find("Attack").transform.Find("Text").GetComponent<Text>().text = unitPw.ToString();
            gObj.transform.Find("Cost").transform.Find("Text").GetComponent<Text>().text = unitCs.ToString();
        }
    }

    public void EndGame()
    {
        if (_endUI != null)
            EndGame(DIFFICULTY.EASY);
    }

    public void EndGame(DIFFICULTY df)
    {
        if (_endUI != null)
        {
            _uiMode = UIMODE.END;
            _difficulty = df;

            if (_isVictory == true && _difficulty == DIFFICULTY.EASY)
                _isClearEasy = true;
            else if (_isVictory == true && _difficulty == DIFFICULTY.NORMAL)
                _isClearNomal = true;
            else if (_isVictory == true && _difficulty == DIFFICULTY.HARD)
                _isClearHard = true;

            _introUI.SetActive(false);
            _playUI.SetActive(false);
            _endUI.SetActive(true);


            if (_isVictory)
                _endUI.transform.Find("ImgVictory").gameObject.SetActive(true);
            else
                _endUI.transform.Find("ImgDefeat").gameObject.SetActive(true);
            _endUI.transform.Find("BtnReset").gameObject.SetActive(true);
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public GameObject GetGameUIObject(string uiName)
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        return _canvas.transform.Find(uiName).gameObject;
    }

    // 난이도 클리어 설정
    void SetActiveClearImage()
    {
        if (_uiMode == UIMODE.INTRO)
        {
            Transform btnEasyTf = _introUI.transform.Find("BtnEasy");
            if (btnEasyTf != null)
            {
                btnEasyTf.gameObject.GetComponent<Button>().enabled = !_isClearEasy;
                btnEasyTf.gameObject.transform.Find("Image").gameObject.SetActive(_isClearEasy);
            }


            Transform btnNormalTf = _introUI.transform.Find("BtnNormal");
            if (btnNormalTf != null)
            {
                    btnNormalTf.gameObject.GetComponent<Button>().enabled = !_isClearNomal;
                btnNormalTf.gameObject.transform.Find("Image").gameObject.SetActive(_isClearNomal);
            }
                

            Transform btnHardTf = _introUI.transform.Find("BtnHard");
            if (btnHardTf != null)
            {
                btnHardTf.gameObject.GetComponent<Button>().enabled = !_isClearHard;
                btnHardTf.gameObject.transform.Find("Image").gameObject.SetActive(_isClearHard);
            }
        }
    }

    // 건물 활성화
    void setActiveBase(bool flag) 
    {
        _baseGroup = GameObject.FindGameObjectWithTag("BaseGroup");
        if (_baseGroup != null)
        {
            GameObject baseBlue = _baseGroup.transform.Find("BaseBlue").gameObject;
            if (baseBlue != null)
                baseBlue.SetActive(flag);

            GameObject baseRed = _baseGroup.transform.Find("BaseRed").gameObject;
            if (baseRed != null)
                baseRed.SetActive(flag);
        }
    }

    // 타이틀셋 애니메이션
    void MoveTitleSet(string titleSet)
    {
        if (titleSet.Equals("GameTitle") && _uiMode == UIMODE.INTRO)
        {
            _titleSetVal = _titleSetVal + 0.015f;
            // Debug.Log("val = " + titleSetVal + " : Mathf.Sin(val) = " + Mathf.Sin(titleSetVal));
            Transform titleTf = _introUI.transform.Find("TitleSet");
            if (titleTf != null)
                titleTf.Translate(new Vector3(0.0f, Mathf.Sin(_titleSetVal) * 0.15f, 0.0f));
        }

        if (titleSet.Equals("GameVictory") && _uiMode == UIMODE.END)
        {
            _titleSetVal = _titleSetVal + 0.015f;
            Transform titleTf = _endUI.transform.Find("ImgVictory");
            if (titleTf != null)
                titleTf.Translate(new Vector3(0.0f, Mathf.Sin(_titleSetVal) * 0.15f, 0.0f));
        }

        if (titleSet.Equals("GameDefeat") && _uiMode == UIMODE.END)
        {
            _titleSetVal = _titleSetVal + 0.015f;
            Transform titleTf = _endUI.transform.Find("ImgDefeat");
            if (titleTf != null)
                titleTf.Translate(new Vector3(0.0f, Mathf.Sin(_titleSetVal) * 0.15f, 0.0f));
        }
    }

    // 카메라 위치 설정
    public void MoveCameraPosX(POINTER pt, DIRECTION dir)
    {
        if (_uiMode == UIMODE.PLAY)
        {
            int speed = _cfgMgr.cameraSpeed;

            if (dir == DIRECTION.LEFT)
            {
                if (pt == POINTER.DOWN)
                    _cameraPosX -= speed;
                else if (pt == POINTER.UP)
                    _cameraPosX += speed;
            }
            else if (dir == DIRECTION.RIGHT)
            {
                if (pt == POINTER.DOWN)
                    _cameraPosX += speed;
                else if (pt == POINTER.UP)
                    _cameraPosX -= speed;
            }
        }

        // Debug.LogFormat("POINTER : {0},  DIRECTION : {1}, _cameraPosX : {2}", pt.ToString() , dir.ToString(), _cameraPosX);

    }

    //카메라 위치 이동
    void MoveCamera()
    {
        if (_uiMode == UIMODE.PLAY)
        {   
            // 카메라 좌우측 최소/최대 위치 지정
            Vector3 cameraPos = _camera.transform.position;
            float posX = cameraPos.x;
            _isCarmeraMove = true;

            if (posX <= _cfgMgr.cameraMinPosx)
            {
                posX = _cfgMgr.cameraMinPosx;
                _isCarmeraMove = false;
            }
                
                
            if (posX >= _cfgMgr.cameraMaxPosX)
            {
                posX = _cfgMgr.cameraMaxPosX;
                _isCarmeraMove = false;
            }
                
            _camera.transform.position = new Vector3(posX, cameraPos.y, cameraPos.z);

            // 카메라 이동
            _camera.transform.Translate(new Vector3(_cameraPosX * Time.deltaTime, 0.0f, 0.0f));
        }   
    }


    // 배경 위치 이동
    void MoveBackground()
    {
        _backGroup = GameObject.FindGameObjectWithTag("BackGroup");
        if (_backGroup != null && _isCarmeraMove == true)
        {
            GameObject backTree = _backGroup.transform.Find("Tree").gameObject;
            if (backTree != null)
                backTree.transform.Translate(new Vector3(_cameraPosX * _cfgMgr.bgTreeSpeed * Time.deltaTime, 0.0f, 0.0f));

            GameObject backForrest = _backGroup.transform.Find("Forrest").gameObject;
            if (backForrest != null)
                backForrest.transform.Translate(new Vector3(_cameraPosX * _cfgMgr.bgForrestSpeed * Time.deltaTime, 0.0f, 0.0f));
                
            GameObject backSky = _backGroup.transform.Find("Sky").gameObject;
            if (backSky != null)
                backSky.transform.Translate(new Vector3(_cameraPosX * _cfgMgr.bgSkySpeed * Time.deltaTime, 0.0f, 0.0f));
                
        }
    }

    // 유닛 구매
    public void BuyUnit(string gameObjNm, UNIT_CLASS uc)
    {
        GameObject gUnitObj  = GameObject.FindGameObjectWithTag("Unit");
        // GameObject sourceObj = Resources.Load<GameObject>(_cfgMgr.defaultPrefabUnitPath + gameObjNm);
        GameObject sourceObj = Resources.Load(_cfgMgr.defaultPrefabUnitPath + gameObjNm) as GameObject;

        _targetObj = Instantiate(sourceObj, gUnitObj.transform);
        _targetObj.name = gameObjNm;
        _targetObj.SetActive(false);

        if (gameObjNm.Substring(0, 1).Equals("B"))
        {
            if (_cost >= _cfgMgr._cost[uc])
            {
                _cost = _cost - _cfgMgr._cost[uc];
                _targetObj.SetActive(true);
                SetPortrait(gameObjNm, uc);
                _sndMgr.Play("Buy");
            }
            else
                Destroy(_targetObj);
        }
        else
        {
            _targetObj.SetActive(true);
            // _sndMgr.Play("Buy");
        }
    }

    // 유닛 업그레이드
    public void UpgradeUnit(string gameObjNm, UNIT_CLASS uc)
    {
        GameObject gUnitObj = GameObject.FindGameObjectWithTag("Unit");
        GameObject sourceObj = Resources.Load(_cfgMgr.defaultPrefabUnitPath + gameObjNm) as GameObject;

        _targetObj = Instantiate(sourceObj, gUnitObj.transform);
        _targetObj.name = gameObjNm;
        _targetObj.SetActive(false);

        if (gameObjNm.Substring(0, 1).Equals("B"))
        {
            if (_cost >= _cfgMgr._cost[uc] * _cfgMgr._upgradeCost)
            {
                _cost = _cost - _cfgMgr._cost[uc] * _cfgMgr._upgradeCost;
                _targetObj.SetActive(true);
                SetPortrait(gameObjNm, uc);
                _sndMgr.Play("Upgrade");
            }
            else
                Destroy(_targetObj);
        }
        else
        {
            _targetObj.SetActive(true);
            // _sndMgr.Play("Upgrade");
        }
    }

    public void DestoryAllUnit()
    {
        // Unit 삭제
        Transform[] childTfList = GameObject.Find("Unit").gameObject.GetComponentsInChildren<Transform>();
        if ( childTfList is not null)
        {
            // Debug.Log("gameObject Length : " + childTfList.Length);
            // UNIT 게임오브젝트는 삭제하지 않는다.
            for (int i = 1; i < childTfList.Length; i++)
            {
                // Debug.Log("gameObject NM : " + childTfList[i].gameObject.name);
                Destroy(childTfList[i].gameObject);
            }
        }

        // HPBar 삭제
        Transform[] playUITfList = _playUI.GetComponentsInChildren<Transform>();
        if (playUITfList is not null)
        {
            // Debug.Log("gameObject Length : " + playUITfList.Length);
            for (int i = 1; i < playUITfList.Length; i++)
            {
                // Debug.Log("gameObject NM : " + playUITfList[i].gameObject.name);
                if (playUITfList[i].gameObject.name.EndsWith("Base") || playUITfList[i].gameObject.name.EndsWith("Clone") )      
                    Destroy(playUITfList[i].gameObject);
            }
        }
    }
}
