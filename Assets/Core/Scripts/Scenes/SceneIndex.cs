/// <summary>
/// シーンのビルドインデックス
/// </summary>
static public class SceneIndex
{
    /// <summary>
    /// LoadSceneMode.Singleで読み込むシーン
    /// </summary>
    public enum Main
    {
        Title = 1,
        StageSelection = 2,
        Game = 3,
    }

    /// <summary>
    /// LoadSceneMode.Additiveで読み込むシーン
    /// </summary>
    public enum Sub
    {
        Stage000 = 4,
    }
}
