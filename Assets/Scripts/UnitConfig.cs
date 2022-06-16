using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitConfig
{
    public float _speed = 1.0f;
    public float _attackRange = 0.7f; // 0.4f 2.5f 3.5f
    public int _hp = 0;
    public int _maxHp = 1000;
    public int _power = 1;
    public GameObject[] _enemyObj = null;

    public TEAM _team = TEAM.BLUE;
    public UNIT_CLASS _unitClass = UNIT_CLASS.SWORD;
    public WEAPON _weapon = WEAPON.SWORD;
}
