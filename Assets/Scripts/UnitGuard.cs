using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitGuard : Unit
{
    protected override void InitUnitConfig()
    {
        // 유닛클래스설정
        _unitConfig._unitClass = UNIT_CLASS.GUARD;

        // 기본값 설정
        _unitConfig._speed = 1.0f;
        _unitConfig._attackRange = 0.4f; // 0.4f 2.5f 3.5f
        _unitConfig._maxHp = 2000;
        _unitConfig._power = 1;

        // 에너지 설정
        _unitConfig._hp = _unitConfig._maxHp;
    }

    protected override void InitWeaponConfig()
    {
        _weaponConfig._speed = 0.0f;
        _weaponConfig._weapon = WEAPON.SHIELD;
        _weaponConfig._damage = 0;
    }
}
