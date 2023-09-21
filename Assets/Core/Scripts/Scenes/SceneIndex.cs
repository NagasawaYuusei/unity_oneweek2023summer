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
        Game = 2,
        Result = 3,
    }

    /// <summary>
    /// LoadSceneMode.Additiveで読み込むシーン
    /// </summary>
    public enum Sub
    {
        GameOver = 4,
        GameEnemy = 5,
        GameBack = 6,
        TitleOption = 7,
    }
}
