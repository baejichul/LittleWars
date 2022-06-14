using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    protected float _tmpSpeed;
    protected bool _isAttacking = false;

    protected GameManager _gMgr;
    protected UnitConfig _unitConfig;
    protected GameObject _dust;
    protected Animator _ani;
    protected ParticleSystem _ps;

    // Start is called before the first frame update
    void Start()
    {
        initUnit();
    }

    // Update is called once per frame
    void Update()
    {
        doMove();
    }

    // 유닛 초기화
    void initUnit()
    {
        _gMgr       = FindObjectOfType<GameManager>();
        _unitConfig = gameObject.AddComponent<UnitConfig>();

        InitUnitConfig();

        // 파티클 설정
        if (transform.Find("Dust") != null)
        {
            _dust = transform.Find("Dust").gameObject;
            _ps = _dust.GetComponent<ParticleSystem>();
        }

        // 애니메이터 설정
        _ani = GetComponent<Animator>();
    }

    protected virtual void InitUnitConfig()
    {
        // 종족설정
        _unitConfig._unitClass = UNIT_CLASS.SWORD;

        // 기본값 설정
        _unitConfig._speed = 1.0f;
        _unitConfig._attackRange = 0.7f; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 1000;
        _unitConfig._power = 1;

        // 에너지 설정
        _unitConfig._hp = _unitConfig._maxHp;

        // 팀 및 이동속도설정
        if (gameObject.layer == (int)TEAM.BLUE)
        {
            _unitConfig._team = TEAM.BLUE;
            _unitConfig._enemyObj = GameObject.FindGameObjectsWithTag(TEAM.RED.ToString());
        }

        if (gameObject.layer == (int)TEAM.RED)
        {
            _unitConfig._team = TEAM.RED;
            _unitConfig._enemyObj = GameObject.FindGameObjectsWithTag(TEAM.BLUE.ToString());
            _unitConfig._speed = _unitConfig._speed * -1.0f;
        }
    }

    // 유닛 이동
    void doMove()
    {
        if (_ps != null && _gMgr._uiMode == UIMODE.PLAY)
        {
            if (_unitConfig._enemyObj != null && _unitConfig._enemyObj.Length > 0)
            {
                GameObject enemy = FindEnemy();
                if (enemy != null) doAttack(enemy);
            }

            if (_isAttacking == false)
            {
                gameObject.transform.Translate(_unitConfig._speed * Time.deltaTime, 0.0f, 0.0f);
                // ParticleSystem.EmissionModule em = _ps.emission;
                // em.enabled = false;
                if (!_ps.isEmitting)
                    _ps.Play();
            }
        }
    }

    void doStop(Collision2D collision)
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

        ParticleSystem ps = gameObject.transform.Find("Dust").GetComponent<ParticleSystem>();
        if (!ps.isEmitting)
            ps.Stop();
    }

    void SetSpeed()
    {
        _unitConfig._speed = _tmpSpeed;
        // Debug.LogFormat("Nm : {0}, _speed : {1}", gameObject.name, _speed);
    }

    // 가장 가까이 있는 적 찾기
    GameObject FindEnemy()
    {
        float? minPosX = null;
        GameObject rtnObj = null;

        Vector3 myObj = gameObject.transform.position;      
        float myPosX = myObj.x;
        
        foreach (GameObject obj in _unitConfig._enemyObj)
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

    // 공격
    void doAttack(GameObject enemyObj)
    {
        // 거리 측정
        Vector3 vecObj = gameObject.transform.position;
        float myPos = vecObj.x;

        Vector3 vecCol = enemyObj.transform.position;
        float colPos = vecCol.x;

        if (_unitConfig._attackRange >= Mathf.Abs(myPos - colPos))
        {
            // Debug.LogFormat("gameObject : {0}, enemyObj: {1}", gameObject.name, enemyObj.name);
            _ani.SetBool("LWAttack", true);
            _isAttacking = true;
            doDamage(enemyObj, _unitConfig._power);
        }
        else
        {
            _ani.SetBool("LWAttack", false);
        }
    }

    // 데미지
    void doDamage(GameObject enemyObj, int damage)
    {
        Unit enemyUnit = enemyObj.GetComponent<Unit>();
        enemyUnit._unitConfig._hp = enemyUnit._unitConfig._hp - damage;
        enemyUnit._unitConfig._hp = Math.Max(enemyUnit._unitConfig._hp, 0);
        enemyUnit._ani.SetTrigger("LWHit");

        if (enemyUnit._unitConfig._hp == 0)
            doDie(enemyUnit, enemyObj);
            
    }

    // 사망
    void doDie(Unit enemyUnit, GameObject enemyObj)
    {
        enemyUnit._ani.SetBool("LWDie", true);
        _isAttacking = false;

        List<GameObject> list = new List<GameObject>(_unitConfig._enemyObj);
        list.Remove(enemyObj);
        _unitConfig._enemyObj = list.ToArray();


        // enemyObj.GetComponent<BoxCollider2D>().enabled = false;
        // enemyObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Destroy(enemyObj,1.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체가 아군이라면
        if ( collision.gameObject.layer == (int)_unitConfig._team )            
            doStop(collision);
    }

}
