using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSword : Unit
{
    protected override void InitUnitConfig()
    {
        // ��������
        _unitConfig._unitClass = UNIT_CLASS.SWORD;

        // �⺻�� ����
        _unitConfig._speed = 1.0f;
        _unitConfig._attackRange = 0.7f; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 1000;
        _unitConfig._power = 3;

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
