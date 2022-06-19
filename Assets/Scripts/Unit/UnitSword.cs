using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSword : Unit
{
    public override void InitUnitConfig(int level)
    {
        // 유닛클래스설정
        _unitConfig._unitClass = UNIT_CLASS.SWORD;

        // 기본값 설정
        _unitConfig._speed = 1.0f + (level - 1) * 0.1f;
        _unitConfig._attackRange = 1.0f + (level - 1) * 0.2f;
        _unitConfig._maxHp = 100000 + (level - 1) * 10000;
        _unitConfig._power = 3 + (level - 1) * 1;
        _unitConfig._level = level;

        // 에너지 설정
        _unitConfig._hp = _unitConfig._maxHp;
    }

    public override void InitWeaponConfig(int level)
    {
        _weaponConfig._speed = 0.0f;
        _weaponConfig._weapon = WEAPON.SWORD;
        _weaponConfig._damage = 0;
    }

}
