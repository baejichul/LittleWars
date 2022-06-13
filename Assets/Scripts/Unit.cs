using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    float _speed = 1.0f;
    float _attackRange = 0.7f; // 0.4f 2.5f 3.5f
    public int _hp = 0;
    public int _maxHp = 100;
    public int _power = 1;
    GameObject[] _enemyObj;

    bool _isAttacking = false;

    TEAM _team;
    UNIT_CLASS _unitClass;

    GameManager _gMgr;
    GameObject _dust;
    Animator _ani;
    ParticleSystem _ps;

    // Start is called before the first frame update
    void Start()
    {
        initUnit();
    }

    // Update is called once per frame
    void Update()
    {   
        move();
    }

    void initUnit()
    {
        _gMgr = FindObjectOfType<GameManager>();
        // 팀 및 이동속도설정
        if (gameObject.layer == (int)TEAM.BLUE)
        {
            _team = TEAM.BLUE;
            _enemyObj = GameObject.FindGameObjectsWithTag(TEAM.RED.ToString());
        }   

        if (gameObject.layer == (int)TEAM.RED)
        {
            _team = TEAM.RED;
            _enemyObj = GameObject.FindGameObjectsWithTag(TEAM.BLUE.ToString());
            _speed = _speed * -1.0f;
        }

        // 종족설정
        _unitClass = UNIT_CLASS.SWORD;

        // 에너지 설정
        _hp = _maxHp;

        // 파티클 설정
        if(transform.Find("Dust") != null)
        {
            _dust = transform.Find("Dust").gameObject;
            _ps = _dust.GetComponent<ParticleSystem>();
        }

        // 애니메이터 설정
        _ani = GetComponent<Animator>();
        _ani.SetTrigger("doMove");
    }

    void move()
    {       
        if (_ps != null && _gMgr._uiMode == UIMODE.PLAY)
        {   
            if (_enemyObj != null && _enemyObj.Length > 0)
            {
                GameObject enemy = FindEnemy();
                doAttack(enemy);
            }

            if (_isAttacking == false)
            {
                gameObject.transform.Translate(_speed * Time.deltaTime, 0.0f, 0.0f);
                // ParticleSystem.EmissionModule em = _ps.emission;
                // em.enabled = false;
                if (!_ps.isEmitting)
                    _ps.Play();
            }
        }            
    }

    void stop()
    {
        if (_ps != null && _gMgr._uiMode == UIMODE.PLAY)
        {
            _ps.Stop();
        }            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.LogFormat("gameObject : {0} ,collision : {1}", gameObject.name, collision.gameObject.name);

    }

    // 가장 가까이 있는 적을 찾는다.
    GameObject FindEnemy()
    {
        float? minPosX = null;
        GameObject rtnObj = null;

        Vector3 myObj = gameObject.transform.position;      
        float myPosX = myObj.x;
        foreach ( GameObject obj in _enemyObj)
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
        return rtnObj;
    }

    void doAttack(GameObject enemyObj)
    {
        // 거리 측정
        Vector3 vecObj = gameObject.transform.position;
        float myPos = vecObj.x;

        Vector3 vecCol = enemyObj.transform.position;
        float colPos = vecCol.x;

        if (_attackRange >= Mathf.Abs(myPos - colPos))
        {
            _ani.SetTrigger("doAttack");
            _isAttacking = true;
            doDamage(enemyObj, _power);
        }
    }

    void doDamage(GameObject enemyObj, int damage)
    {
        Unit enemyUnit = enemyObj.GetComponent<Unit>();
        enemyUnit._hp = enemyUnit._hp - damage;
        enemyUnit._hp = Math.Max(enemyUnit._hp, 0);

        if (enemyUnit._hp == 0 )
            doDie(enemyObj);
    }

    void doDie(GameObject enemyObj)
    {
        _isAttacking = false;

        List<GameObject> list = new List<GameObject>(_enemyObj);
        list.Remove(enemyObj);
        _enemyObj = list.ToArray();

        Destroy(enemyObj);
    }

}
