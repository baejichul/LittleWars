using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIMODE : uint
{
    INTRO,
    PLAY,
    END
}
public enum DIFFICULTY : uint
{
    EASY,
    NORMAL,
    HARD
}

public enum POINTER : uint
{   
    UP,
    DOWN
}

public enum DIRECTION : uint
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public enum UNIT_CLASS : uint
{
    SWORD,
    ARCHER,
    GUARD,
    WIZARD
}

public enum TEAM : uint
{
    BLUE = 8,
    RED
}

public enum WEAPON : uint
{
    SWORD,
    SHIELD,
    ARROW,
    MAGIC
}

public class ConfigManager : MonoBehaviour
{
    // camera
    public int cameraSpeed {get; set;} = 2;
    public float cameraMinPosx { get; set; } = -1.85f;
    public float cameraMaxPosX { get; set; } = 1.85f;

    // background
    public float bgTreeSpeed { get; set; } = 0.25f;
    public float bgForrestSpeed { get; set; } = 0.5f;
    public float bgSkySpeed { get; set; } = 0.9f;

    // AudioSource
    public string audSrcBGM { get; set; } = "BGM";
    public string audSrcBuy { get; set; } = "Buy";
    public string audSrcDefeat { get; set; } = "Defeat";
    public string audSrcUpgrade { get; set; } = "Upgrade";
    public string audSrcVictory { get; set; } = "Victory";
    public string audSrcAttack { get; set; } = "Attack";


}
