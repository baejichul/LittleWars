using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitGuard : Unit
{
    protected override void InitUnitConfig(int level)
    {
        // 유닛클래스설정
        _unitConfig._unitClass = UNIT_CLASS.GUARD;

        // 기본값 설정
        _unitConfig._speed = 1.0f + (level - 1) * 0.1f;
        _unitConfig._attackRange = 0.4f + (level - 1) * 0.2f;
        _unitConfig._maxHp = 200000 + (level - 1) * 10000;
        _unitConfig._power = 1 + (level - 1) * 1;

        // 에너지 설정
        _unitConfig._hp = _unitConfig._maxHp;
    }

    protected override void InitWeaponConfig(int level)
    {
        _weaponConfig._speed = 0.0f;
        _weaponConfig._weapon = WEAPON.SHIELD;
        _weaponConfig._damage = 0;
    }
}
