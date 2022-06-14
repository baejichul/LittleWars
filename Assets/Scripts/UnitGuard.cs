using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitGuard : Unit
{
    protected override void InitUnitConfig()
    {
        // ��������
        _unitConfig._unitClass = UNIT_CLASS.GUARD;

        // �⺻�� ����
        _unitConfig._speed = 1.0f;
        _unitConfig._attackRange = 0.4f; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 2000;
        _unitConfig._power = 1;

        // ������ ����
        _unitConfig._hp = _unitConfig._maxHp;

        // �� �� �̵��ӵ�����
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

}
