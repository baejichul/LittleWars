using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitArcher : Unit
{
    protected override void InitUnitConfig(int level)
    {
        // ����Ŭ��������
        _unitConfig._unitClass = UNIT_CLASS.ARCHER;

        // �⺻�� ����
        _unitConfig._speed = 0.4f * level;
        _unitConfig._attackRange = 5.0f * level; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 50000 * level;
        _unitConfig._power = 0 * level;

        // ������ ����
        _unitConfig._hp = _unitConfig._maxHp;
    }

    protected override void InitWeaponConfig(int level)
    {
        _weaponConfig._speed = 5.0f * level;
        _weaponConfig._weapon = WEAPON.ARROW;
        _weaponConfig._damage = 5000 * level;
    }

    // ����
    protected override void DoAttack(GameObject enemyObj)
    {
        // �Ÿ� ����
        Vector3 vecObj = gameObject.transform.position;
        float myPos = vecObj.x;

        Vector3 vecCol = enemyObj.transform.position;
        float colPos = vecCol.x;

        if (_unitConfig._attackRange >= Mathf.Abs(myPos - colPos))
        {
            //ȭ���� �߻��Ѵ�.
            GameObject Arrow = transform.Find("Arrow").gameObject;
            Arrow.SetActive(true);
            Arrow.transform.Translate(_weaponConfig._speed* Time.deltaTime, 0.0f, 0.0f);

            // Debug.LogFormat("gameObject : {0}, enemyObj: {1}", gameObject.name, enemyObj.name);
            _ani.SetBool("LWAttack", true);
            _isAttacking = true;
        }
        else
        {
            _ani.SetBool("LWAttack", false);
        }
    }
}
