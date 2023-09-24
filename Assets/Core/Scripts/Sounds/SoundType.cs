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

        Result, // kawata:BG扱いですが一度だけ再生とのことでSEの方に持ってきました
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
