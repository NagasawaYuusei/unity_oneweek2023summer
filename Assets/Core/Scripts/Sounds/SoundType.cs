using System;

/// <summary>
/// サウンドの種類
/// </summary>
static public class SoundType
{
    public enum BGM
    {
        FirstWave,
        SecondWave,
        ThirdWave,
        Result,
    }

    public enum SE
    {
        Select, // 決定ボタン

        Do,
        DoDoDo,
        Parry1,
        Parry2,
        Parry3,
        Parry4,
        ParryMiss,
        Atack,
        Hit,
        PlayerDead1,
        PlayerDeadWind,
        PlayerDown,
        Zu,
        Kisatsu,
        DelayZako,
        DelayBoss,
        BeforeBoss,
        AttackBoss,
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
