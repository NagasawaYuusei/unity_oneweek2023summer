using System;
/// <summary>
/// サウンドの種類
/// </summary>
static public class SoundType
{
    public enum BGM
    {
        Title,
        Game,
        Result,
    }

    public enum SE
    {
        Start,
        CountDown,
        End,
        ClickButton,
        ChangeScene
    }
}
