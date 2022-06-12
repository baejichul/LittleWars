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

public class ConfigManager : MonoBehaviour
{
    public int cameraSpeed {get; set;} = 2;
    public float cameraMinPosx { get; set; } = -2.0f;
    public float cameraMaxPosX { get; set; } = 2.0f;



}
