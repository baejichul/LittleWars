using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSword : Unit
{
    protected override void InitUnitConfig(int level)
    {
        // 유닛클래스설정
        _unitConfig._unitClass = UNIT_CLASS.SWORD;

        // 기본값 설정
        _unitConfig._speed = 1.0f * level;
        _unitConfig._attackRange = 0.7f * level; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 100000 * level;
        _unitConfig._power = 3 * level;

        // 에너지 설정
        _unitConfig._hp = _unitConfig._maxHp;
    }

    protected override void InitWeaponConfig(int level)
    {
        _weaponConfig._speed = 0.0f * level;
        _weaponConfig._weapon = WEAPON.SWORD;
        _weaponConfig._damage = 0 * level;
    }

}
