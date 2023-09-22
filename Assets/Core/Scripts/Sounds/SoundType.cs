using System;

/// <summary>
/// サウンドの種類
/// </summary>
static public class SoundType
{
    public enum BGM
    {
        FirstWave,
        ThirdWave,
    }

    public enum SE
    {
        Do,
        DoDoDo,
        Parry1,
        Parry2,
        Parry3,
        Parry4,
        Atack,
        Hit,
        PlayerDead1,
        PlayerDeadWind,
        PlayerDown,
        Zu,
        Kisatsu,
        DelayZako,
        BeforeBoss,
    }

    static public int BGMCount
    {
        get { return Enum.GetValues(typeof(BGM)).Length; }
    }

    static public int SECount
    {
        get { return Enum.GetValues(typeof(SE)).Length; }
    }
}
