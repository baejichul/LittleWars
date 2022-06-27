using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitWizard : Unit
{
    public override void InitUnitConfig(int level)
    {
        // ����Ŭ��������
        _unitConfig._unitClass = UNIT_CLASS.WIZARD;

        // �⺻�� ����
        _unitConfig._speed = 0.4f + (level - 1) * 0.1f;
        _unitConfig._attackRange = 6.0f + (level - 1) * 0.3f;
        _unitConfig._maxHp = 40000 + (level - 1) * 1000;
        _unitConfig._power = 6 + (level - 1) * 1;
        _unitConfig._level = level;

        // ������ ����
        _unitConfig._hp = _unitConfig._maxHp;
    }

    public override void InitWeaponConfig(int level)
    {
        _weaponConfig._speed = 0.0f;
        _weaponConfig._weapon = WEAPON.MAGIC;
        _weaponConfig._damage = 0;
    }

    // ����
    protected override void DoAttack(GameObject enemyObj)
    {
        // ��������ȿ��
        string magicObjNm = "Magic" + _unitConfig._team.ToString();
        GameObject gObj = transform.Find(magicObjNm).gameObject;
        gObj.SetActive(true);

        Vector3 vecCol = enemyObj.transform.position;
        transform.Find(magicObjNm).position = new Vector3(vecCol.x, vecCol.y + 0.75f, vecCol.z);
        if (gObj is not null)
        {
            ParticleSystem psMagic = gObj.GetComponent<ParticleSystem>();
            if (psMagic != null && !psMagic.isEmitting)
                psMagic.Play();
        }

        DoDamage(enemyObj, _unitConfig._power);
    }
}
