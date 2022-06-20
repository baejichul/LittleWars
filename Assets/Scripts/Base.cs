using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public int _hp = 0;
    public int _maxHp = 1000000;
    public TEAM _team = TEAM.BLUE;

    public GameManager _gMgr;
    public SoundManager _sndMgr;
    
    GameObject _hpBarObj;
    public GameObject _targetHpBar;
    public Animator _ani;


    public Vector3 _hpBarOffset;

    // Start is called before the first frame update
    void Start()
    {
        _gMgr   = FindObjectOfType<GameManager>();
        _sndMgr = FindObjectOfType<SoundManager>();
        _ani = GetComponent<Animator>();

        _hp = _maxHp;
        if (gameObject.layer == (int)TEAM.BLUE)
            _team = TEAM.BLUE;

        if (gameObject.layer == (int)TEAM.RED)
            _team = TEAM.RED;

        InitHpBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetHpBar is not null)
            UpdateHpBarPos(_targetHpBar);
    }

    void InitHpBar()
    {
        if (gameObject.layer == (int)TEAM.BLUE)
        {
            _hpBarObj = _gMgr._playUI.transform.Find("HpBarBlue").gameObject;
            _hpBarOffset = new Vector3(-0.25f, 1.8f, 0);
        }
        if (gameObject.layer == (int)TEAM.RED)
        {
            _hpBarObj = _gMgr._playUI.transform.Find("HpBarRed").gameObject;
            _hpBarOffset = new Vector3(0.25f, 1.8f, 0);
        }

        _targetHpBar = Instantiate(_hpBarObj, _gMgr._playUI.transform);
        _targetHpBar.name = _hpBarObj.name + "Base";

        UpdateHpBarPos(_targetHpBar);
    }

    void UpdateHpBarPos(GameObject hpBarObj)
    {
        //Debug.Log(hpBarObj.name);
        Vector3 unitPos = transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitPos + _hpBarOffset);     // 월드좌표를 스크린좌표(UI 좌표)로 변환

        // 변환된 스크린좌표를 체력바의 rectTransform에 적용
        if (hpBarObj != null)
            hpBarObj.transform.position = screenPos;
    }

    public void UpdateHpBar(GameObject hpBarObj)
    {
        if (hpBarObj.transform.Find("Hp") is not null)
            hpBarObj.transform.Find("Hp").gameObject.GetComponent<Image>().fillAmount = (float)_hp / (float)_maxHp;
    }
    

    public void DoDestory(TEAM team)
    {
        _ani.SetBool("LWDie", true);
        _sndMgr.Stop("BGM");
        // Debug.Log("WINNER = " + team.ToString());

        if (team == TEAM.BLUE)
            _sndMgr.Play("Victory");
        else
            _sndMgr.Play("Defeat");

        Destroy(_targetHpBar);
        Destroy(gameObject);

        _gMgr.EndGame(_gMgr._difficulty);
    }
}
