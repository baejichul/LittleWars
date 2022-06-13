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
    RANGE,
    GUARD,
    WIZARD,
    BULLET
}

public enum TEAM : uint
{
    BLUE = 8,
    RED
}

public class ConfigManager : MonoBehaviour
{
    // camera
    public int cameraSpeed {get; set;} = 2;
    public float cameraMinPosx { get; set; } = -2.0f;
    public float cameraMaxPosX { get; set; } = 2.0f;

    // background
    public float bgTreeSpeed { get; set; } = 0.25f;
    public float bgForrestSpeed { get; set; } = 0.5f;
    public float bgSkySpeed { get; set; } = 1.0f;


}
