using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // ScriptableOject String
    public const string BRICK_RED_SO_PATH= "ScriptableObjects/Brick/BrickRedSO";
    public const string BRICK_GREEN_SO_PATH = "ScriptableObjects/Brick/BrickGreenSO";
    public const string BRICK_BLUE_SO_PATH = "ScriptableObjects/Brick/BrickBlueSO";
    public const string BRICK_YELLOW_SO_PATH = "ScriptableObjects/Brick/BrickYellowSO";

    //Animation Parameter String
    public const string ANI_VELOCITY = "velocity";
    public const string ANI_RESULT = "Result";
    public const string ANI_ISFALL = "isFall";

    public const int RESULT_NOTWIN = 0;
    public const int RESULT_WIN = 1;

    //TAG
    public const string TAG_BOT = "Bot";
    public const string TAG_PLAYER = "Player";

    //Layer
    public const string LAYER_GRAY = "Gray";

    //number const
    public const float BRICK_HEIGHT = 0.06f;
    public const int NUMBER_OF_THRESHOLD = 2;

    public const float OFFSET_BRICK_SPAWN_Y = 0.05f;
    public const float OFFSET_BRICK_SPAWN_X = 0.8f;
    public const float OFFSET_BRICK_SPAWN_Z = 0.4f;

    //Find string
    public const string LEVEL_STRING = "Level";
    public const string BRIDGE_STRING = "Bridge";
    public const string CLONE_STRING = "(Clone)";
}
