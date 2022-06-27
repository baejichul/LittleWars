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

    // 유닛 초기화
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
        

        // 파티클 설정
        if (transform.Find("Dust") != null)
        {
            _dust = transform.Find("Dust").gameObject;
            _ps = _dust.GetComponent<ParticleSystem>();
        }

        // 애니메이터 설정
        _ani = GetComponent<Animator>();
    }

    protected void InitUnitConfig()
    {
        // 업그레이드 유닛설정
        String gObjNm = transform.parent.gameObject.name;
        if (gObjNm.EndsWith("U"))
            _unitConfig._level = _unitConfig._level + 1;

        InitUnitConfig(_unitConfig._level);
    }

    public virtual void InitUnitConfig(int level)
    {
        // 유닛클래스설정
        _unitConfig._unitClass = UNIT_CLASS.SWORD;

        // 기본값 설정
        _unitConfig._speed = 1.0f * level;

        _unitConfig._attackRange = 0.7f * level; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 100000 * level;
        _unitConfig._power = 1 * level;
        _unitConfig._level = level;

        // 에너지 설정
        _unitConfig._hp = _unitConfig._maxHp;
    }

    protected void InitWeaponConfig()
    {
        InitWeaponConfig(_unitConfig._level);
    }

    public virtual void InitWeaponConfig(int level)
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
                // 나의 적리스트를 추가한 후 적의 적리스트에 나를 추가한다.
                _unitConfig._enemyObjList.AddRange(enemyArr);
                foreach ( GameObject gObj in enemyArr)
                {
                    if ( !gObj.transform.parent.gameObject.name.Equals("BaseGroup") )
                        gObj.GetComponent<Unit>()._unitConfig._enemyObjList.Add(gameObject);
                }
            }
        }

        if (gameObject.layer == (int)TEAM.RED)
        {
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag(TEAM.BLUE.ToString());
            if (enemyArr != null)
            {
                // 나의 적리스트를 추가한 후 적의 적리스트에 나를 추가한다.
                _unitConfig._enemyObjList.AddRange(enemyArr);
                foreach (GameObject gObj in enemyArr)
                {
                    if (!gObj.transform.parent.gameObject.name.Equals("BaseGroup"))
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
        _targetHpBar.name = _hpBarObj.name + "Clone";
        UpdateHpBarPos(_targetHpBar);
    }

    
    // 유닛 이동
    void DoMove()
    {
        if (_gMgr._uiMode == UIMODE.PLAY)
        {
            if (_targetHpBar is not null)
                UpdateHpBarPos(_targetHpBar);
            
            if (_unitConfig._enemyObjList.Count > 0)
            {
                GameObject enemy = FindEnemy();
                if (enemy != null)
                {
                    float distance = CheckDistance(enemy);
                    if (_unitConfig._attackRange >= distance)
                    {
                        _isAttacking = true;
                        if (enemy.transform.parent.gameObject.name.Equals("Unit"))
                            _ani.SetBool("LWAttack", true);
                        else
                            _ani.SetTrigger("LWBaseAttack");

                        DoAttack(enemy);
                    }
                    else
                    {
                        _isAttacking = false;
                        if (enemy.transform.parent.gameObject.name.Equals("Unit"))
                            _ani.SetBool("LWAttack", false);
                    }
                    // Debug.LogFormat("LWAttack : " + _ani.GetBool("LWAttack"));
                }
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
            // 뒤쪽 유닛만 1초간 정지 시킨다.
            Vector3 gameVec = gameObject.transform.position;
            Vector3 colVec = collision.gameObject.transform.position;

            if (_unitConfig._team == TEAM.BLUE)
            {
                if (gameVec.x < colVec.x)       // 왼쪽
                    StopUnit();
            }
            else
            {
                if (gameVec.x > colVec.x)       // 오른쪽
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

    // 가장 가까이 있는 적 찾기
    GameObject FindEnemy()
    {
        float distance = 0.0f;
        GameObject rtnObj = null;

        Vector3 myObj = gameObject.transform.position;      
        float myPosX = myObj.x;

        int i = 0;
        foreach (GameObject obj in _unitConfig._enemyObjList)
        {
            if (obj != null)
            {
                Vector3 eyObj = obj.transform.position;
                float posX = eyObj.x;

                if (i ==0)
                {
                    distance = Mathf.Abs(myPosX - posX);
                    rtnObj = obj;
                }
                else if (distance > Mathf.Abs(myPosX - posX))
                {
                    distance = Mathf.Abs(myPosX - posX);
                    rtnObj = obj;
                }
                i++;
            }
        }
        // Debug.Log("return rtnObj = " + rtnObj.name);

        return rtnObj;
    }

    // 공격
    protected virtual void DoAttack(GameObject enemyObj)
    {       
        DoDamage(enemyObj, _unitConfig._power);
    }

    float CheckDistance(GameObject enemyObj)
    {
        // 거리 측정
        Vector3 vecObj = gameObject.transform.position;
        float myPos = vecObj.x;

        Vector3 vecCol = enemyObj.transform.position;
        float colPos = vecCol.x;

        // Debug.LogFormat("attackRange : {0}, myPos : {1}, colPos : {2}, distance : {3}", _unitConfig._attackRange, myPos, colPos, Mathf.Abs(myPos - colPos));
        // Debug.LogFormat("Distance : " + Mathf.Abs(myPos - colPos));

        return Mathf.Abs(myPos - colPos);
    }

    // 데미지
    public void DoDamage(GameObject enemyObj, int damage)
    {
        // Debug.Log("enemyObj.Name = " + enemyObj.transform.parent.gameObject.name);
        if ( enemyObj.transform.parent.gameObject.name.Equals("BaseGroup"))
        {
            Base enemyBase = enemyObj.GetComponent<Base>();
            enemyBase._hp = enemyBase._hp - damage;
            enemyBase._hp = Math.Max(enemyBase._hp, 0);
            if (enemyBase._hp == 0)
            {
                _isAttacking = false;
                RemoveEnemy(enemyObj);
                enemyBase.DoDestory(_unitConfig._team);
            }
            else
            {
                enemyBase.UpdateHpBar(enemyBase._targetHpBar);
                enemyBase.UpdateHealthBar(enemyBase._healthBar);
                enemyBase._ani.SetTrigger("LWHit");
            }
        }
        else
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

                if (gameObject.layer == (int)TEAM.BLUE)
                    _sndMgr.PlayAttack(_unitConfig._unitClass);
            }
        }
    }

    // 사망
    void DoDie(Unit enemyUnit, GameObject enemyObj)
    {
        _ani.SetBool("LWAttack", false);
        enemyUnit._ani.SetBool("LWDie", true);
        RemoveEnemy(enemyObj);
        if (gameObject.layer == (int)TEAM.BLUE)
            _sndMgr.Play("Defeat");


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

        Debug.Log($"_enemyObjList.Count = {_unitConfig._enemyObjList.Count}");
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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitPos + _unitConfig._hpBarOffset);     // 월드좌표를 스크린좌표(UI 좌표)로 변환

        // 변환된 스크린좌표를 체력바의 rectTransform에 적용
        if (hpBarObj != null)
            hpBarObj.transform.position = screenPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체가 아군이라면
        if ( collision.gameObject.layer == (int)_unitConfig._team )            
            DoStop(collision);
    }

}
