using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    protected float _tmpSpeed;
    public bool _isAttacking = false;

    protected GameManager _gMgr;
    public SoundManager _sndMgr;
    public UnitConfig _unitConfig;
    public WeaponConfig _weaponConfig;

    protected GameObject _dust;
    protected Animator _ani;
    protected ParticleSystem _ps;
    GameObject _hpBarObj;
    public GameObject _targetHpBar;

    // Start is called before the first frame update
    void Start()
    {
        initUnit();
    }

    // Update is called once per frame
    void Update()
    {
        DoMove();
    }

    // ���� �ʱ�ȭ
    void initUnit()
    {
        _gMgr   = FindObjectOfType<GameManager>();
        _sndMgr = FindObjectOfType<SoundManager>();
        // _unitConfig = gameObject.AddComponent<UnitConfig>();
        _unitConfig = new UnitConfig();

        InitUnitConfig();
        InitWeaponConfig();
        InitTeam();
        InitEnemyList();
        InitHpBar();
        initRedTeam();
        

        // ��ƼŬ ����
        if (transform.Find("Dust") != null)
        {
            _dust = transform.Find("Dust").gameObject;
            _ps = _dust.GetComponent<ParticleSystem>();
        }

        // �ִϸ����� ����
        _ani = GetComponent<Animator>();
    }

    protected void InitUnitConfig()
    {
        // ���׷��̵� ���ּ���
        String gObjNm = transform.parent.gameObject.name;
        if (gObjNm.EndsWith("U"))
            _unitConfig._level = _unitConfig._level + 1;

        InitUnitConfig(_unitConfig._level);
    }

    protected virtual void InitUnitConfig(int level)
    {
        // ����Ŭ��������
        _unitConfig._unitClass = UNIT_CLASS.SWORD;

        // �⺻�� ����
        _unitConfig._speed = 1.0f * level;

        _unitConfig._attackRange = 0.7f * level; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 100000 * level;
        _unitConfig._power = 1 * level;

        // ������ ����
        _unitConfig._hp = _unitConfig._maxHp;
    }

    protected void InitWeaponConfig()
    {
        InitWeaponConfig(_unitConfig._level);
    }

    protected virtual void InitWeaponConfig(int level)
    {
        _weaponConfig._speed = 0 * level;
        _weaponConfig._weapon = WEAPON.SWORD;
        _weaponConfig._damage = 0 * level;
    }

    void InitTeam()
    {   
        if (gameObject.layer == (int)TEAM.BLUE)
            _unitConfig._team = TEAM.BLUE;

        if (gameObject.layer == (int)TEAM.RED)
            _unitConfig._team = TEAM.RED;
    }

    void InitEnemyList()
    {
        _unitConfig._enemyObjList = new List<GameObject>();

        if (gameObject.layer == (int)TEAM.BLUE)
        {
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag(TEAM.RED.ToString());
            if (enemyArr != null)
            {
                // ���� ������Ʈ�� �߰��� �� ���� ������Ʈ�� ���� �߰��Ѵ�.
                _unitConfig._enemyObjList.AddRange(enemyArr);
                foreach ( GameObject gObj in enemyArr)
                {
                    gObj.GetComponent<Unit>()._unitConfig._enemyObjList.Add(gameObject);
                }
            }
        }

        if (gameObject.layer == (int)TEAM.RED)
        {
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag(TEAM.BLUE.ToString());
            if (enemyArr != null)
            {
                // ���� ������Ʈ�� �߰��� �� ���� ������Ʈ�� ���� �߰��Ѵ�.
                _unitConfig._enemyObjList.AddRange(enemyArr);
                foreach (GameObject gObj in enemyArr)
                {
                    gObj.GetComponent<Unit>()._unitConfig._enemyObjList.Add(gameObject);
                }
            }   
        }
    }

    void initRedTeam()
    {
        if (gameObject.layer == (int)TEAM.RED)
        {
            _unitConfig._speed = _unitConfig._speed * -1.0f;
            _weaponConfig._speed = _weaponConfig._speed * -1.0f;
            _unitConfig._hpBarOffset.x = _unitConfig._hpBarOffset.x * -1.0f;
        }
            
    }

    void InitHpBar()
    {
        if (gameObject.layer == (int)TEAM.BLUE)
            _hpBarObj = _gMgr._playUI.transform.Find("HpBarBlue").gameObject;
        if (gameObject.layer == (int)TEAM.RED)
            _hpBarObj = _gMgr._playUI.transform.Find("HpBarRed").gameObject;

        _targetHpBar = Instantiate(_hpBarObj,_gMgr._playUI.transform);
        _targetHpBar.name = _hpBarObj.name;
        UpdateHpBarPos(_targetHpBar);
    }

    
    // ���� �̵�
    void DoMove()
    {
        if (_gMgr._uiMode == UIMODE.PLAY)
        {
            if (_targetHpBar is not null)
                UpdateHpBarPos(_targetHpBar);
            

            // �׸���� �Ŀ��� _unitConfig._enemyObjList.Count 1�� �Ǵ� ���� �߻�
            if (_unitConfig._enemyObjList.Count > 0)
            {
                GameObject enemy = FindEnemy();
                if (enemy != null)
                    DoAttack(enemy);
            }
            
            
            if (_isAttacking == false)
            {
                gameObject.transform.Translate(_unitConfig._speed * Time.deltaTime, 0.0f, 0.0f);
                // ParticleSystem.EmissionModule em = _ps.emission;
                // em.enabled = false;
                if (_ps != null && !_ps.isEmitting)
                    _ps.Play();
            }
        }
    }

    void DoStop(Collision2D collision)
    {
        if (_gMgr._uiMode == UIMODE.PLAY)
        {
            // ���� ���ָ� 1�ʰ� ���� ��Ų��.
            Vector3 gameVec = gameObject.transform.position;
            Vector3 colVec = collision.gameObject.transform.position;

            if (_unitConfig._team == TEAM.BLUE)
            {
                if (gameVec.x < colVec.x)       // ����
                    StopUnit();
            }
            else
            {
                if (gameVec.x > colVec.x)       // ������
                    StopUnit();
            }
        }
    }

    void StopUnit()
    {
        if (_unitConfig._speed != 0.0f)
        {
            _tmpSpeed = _unitConfig._speed;
            _unitConfig._speed = 0.0f;
            Invoke("SetSpeed", 1.0f);
        }

        // ParticleSystem ps = gameObject.transform.Find("Dust").GetComponent<ParticleSystem>();
        // if (!ps.isEmitting)
        // ps.Stop();
    }

    void SetSpeed()
    {
        _unitConfig._speed = _tmpSpeed;
        // Debug.LogFormat("Nm : {0}, _speed : {1}", gameObject.name, _speed);
    }

    // ���� ������ �ִ� �� ã��
    GameObject FindEnemy()
    {
        float? minPosX = null;
        GameObject rtnObj = null;

        Vector3 myObj = gameObject.transform.position;      
        float myPosX = myObj.x;

        foreach (GameObject obj in _unitConfig._enemyObjList)
        {
            if (obj != null)
            {
                Vector3 eyObj = obj.transform.position;
                float posX = eyObj.x;
                if (!minPosX.HasValue)
                {
                    minPosX = Mathf.Abs(myPosX - posX);
                    rtnObj = obj;
                }
                else if (minPosX > Mathf.Abs(myPosX - posX))
                {
                    minPosX = Mathf.Abs(myPosX - posX);
                    rtnObj = obj;
                }
            }
        }
        
        return rtnObj;
    }

    // ����
    protected virtual void DoAttack(GameObject enemyObj)
    {
        // �Ÿ� ����
        Vector3 vecObj = gameObject.transform.position;
        float myPos = vecObj.x;

        Vector3 vecCol = enemyObj.transform.position;
        float colPos = vecCol.x;

        if (_unitConfig._attackRange >= Mathf.Abs(myPos - colPos))
        {
            // Debug.LogFormat("gameObject : {0}, enemyObj: {1}", gameObject.name, enemyObj.name);
            _ani.SetBool("LWAttack", true);
            _isAttacking = true;
            DoDamage(enemyObj, _unitConfig._power);
        }
        else
        {
            _ani.SetBool("LWAttack", false);
            _isAttacking = false;
        }
    }

    // ������
    public void DoDamage(GameObject enemyObj, int damage)
    {
        Unit enemyUnit = enemyObj.GetComponent<Unit>();
        // UnitConfig enemyUC = enemyObj.GetComponent<UnitConfig>();
        enemyUnit._unitConfig._hp = enemyUnit._unitConfig._hp - damage;
        enemyUnit._unitConfig._hp = Math.Max(enemyUnit._unitConfig._hp, 0);
        // Debug.Log(gameObject.name + " => damage = " + damage + " : _hp = " + enemyUnit._unitConfig._hp);

        if (enemyUnit._unitConfig._hp == 0)
        {
            _isAttacking = false;
            DoDie(enemyUnit, enemyObj);
        }
        else
        {
            UpdateHpBar(enemyUnit._targetHpBar, enemyUnit._unitConfig);

            enemyUnit._ani.SetTrigger("LWHit");
            // _sndMgr.PlayAttack(_unitConfig._unitClass);
        }


    }

    // ���
    void DoDie(Unit enemyUnit, GameObject enemyObj)
    {
        enemyUnit._ani.SetBool("LWDie", true);
        RemoveEnemy(enemyObj);
        _sndMgr.Play("Deafeat");


        // enemyObj.GetComponent<BoxCollider2D>().enabled = false;
        // enemyObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Destroy(enemyUnit._targetHpBar);
        Destroy(enemyObj,1.0f);
    }

    public void AddEnemy(GameObject enemyObj)
    {   
        _unitConfig._enemyObjList.Add(enemyObj);
    }

    void RemoveEnemy(GameObject enemyObj)
    {   
        _unitConfig._enemyObjList.Remove(enemyObj);
    }

    void UpdateHpBar(GameObject hpBarObj, UnitConfig unitConfig)
    {
        if (hpBarObj.transform.Find("Hp") is not null)
        {
            hpBarObj.transform.Find("Hp").gameObject.GetComponent<Image>().fillAmount = (float)unitConfig._hp / (float)unitConfig._maxHp;
            // Debug.LogFormat("hpBarObj {0} : fillAmount {1}", hpBarObj.name, hpBarObj.transform.Find("Hp").gameObject.GetComponent<Image>().fillAmount);
        }
            
    }

    void UpdateHpBarPos(GameObject hpBarObj)
    {
        //Debug.Log(hpBarObj.name);
        Vector3 unitPos = transform.position;       
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitPos + _unitConfig._hpBarOffset);     // ������ǥ�� ��ũ����ǥ(UI ��ǥ)�� ��ȯ

        // ��ȯ�� ��ũ����ǥ�� ü�¹��� rectTransform�� ����
        if (hpBarObj != null)
            hpBarObj.transform.position = screenPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹ü�� �Ʊ��̶��
        if ( collision.gameObject.layer == (int)_unitConfig._team )            
            DoStop(collision);
    }

}