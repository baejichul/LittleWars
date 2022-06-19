using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool _isClearEasy;
    public bool _isClearNomal;
    public bool _isClearHard;

    public ConfigManager _cfgMgr;
    public SoundManager _sndMgr;
    GameObject _canvas;
    GameObject _baseGroup;
    GameObject _backGroup;

    public GameObject _introUI;
    public GameObject _playUI;
    public GameObject _endUI;

    public GameObject _camera;

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
        _camera = GameObject.FindGameObjectWithTag("MainCamera");

        InitGameUIObject();
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {   
        MoveTitleSet();
        MoveCamera();
        SetCostBar();
    }

    private void LateUpdate()
    {
        MoveBackground();
    }

    void InitGame()
    {   
        if (_introUI != null)
            InitGame(DIFFICULTY.EASY);
    }

    void InitGame(DIFFICULTY df)
    {   
        if (_introUI != null)
        {
            _uiMode = UIMODE.INTRO;
            _difficulty = df;

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
            _uiMode = UIMODE.PLAY;
            _difficulty = df;
            
            _introUI.SetActive(false);
            _playUI.SetActive(true);
            _endUI.SetActive(false);

            setActiveBase(true);
            // StartCoroutine("IncreaseCost");
            // StartCoroutine(IncreaseCost());
            Invoke("IncreaseCostNow", 2.0f);

            imgCost = _playUI.transform.Find("ControlSet").transform.Find("ImgCost");
            if (imgCost is not null)
            {
                Vector2 vec = imgCost.Find("Bar").GetComponent<RectTransform>().sizeDelta;
                _maxBarwidth = vec.x;               // 최대값 저장

                imgCost.Find("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, vec.y);      // 0으로 초기화
            }
            
        }
    }

    // 2초 간격으로 Cost 증가
    IEnumerator IncreaseCost()
    {   
        while (_cost < _maxCost)
        {
            yield return new WaitForSeconds(2.0f);
            _cost = _cost + 1;
            SetCostBar();
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

            _introUI.SetActive(false);
            _playUI.SetActive(false);
            _endUI.SetActive(true);
        }
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
    void MoveTitleSet()
    {
        if (_uiMode == UIMODE.INTRO)
        {
            _titleSetVal = _titleSetVal + 0.015f;
            // Debug.Log("val = " + titleSetVal + " : Mathf.Sin(val) = " + Mathf.Sin(titleSetVal));
            Transform titleTf = _introUI.transform.Find("TitleSet");
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

        GameObject targetObj = Instantiate(sourceObj, gUnitObj.transform);
        targetObj.name = gameObjNm;
        targetObj.SetActive(false);

        if (_cost >= _cfgMgr._cost[uc])
        {
            _cost = _cost - _cfgMgr._cost[uc];
            targetObj.SetActive(true);
            _sndMgr.Play("Buy");
        }   
        else
            Destroy(targetObj);

    }

    // 유닛 업그레이드
    public void UpgradeUnit(string gameObjNm, UNIT_CLASS uc)
    {
        GameObject gUnitObj = GameObject.FindGameObjectWithTag("Unit");
        GameObject sourceObj = Resources.Load(_cfgMgr.defaultPrefabUnitPath + gameObjNm) as GameObject;

        GameObject targetObj = Instantiate(sourceObj, gUnitObj.transform);
        targetObj.name = gameObjNm;
        targetObj.SetActive(false);
        
        if (_cost >= _cfgMgr._cost[uc] * _cfgMgr._upgradeCost)
        {
            _cost = _cost - _cfgMgr._cost[uc] * _cfgMgr._upgradeCost;
            targetObj.SetActive(true);
            _sndMgr.Play("Upgrade");
        }   
        else
            Destroy(targetObj);
    }
}
