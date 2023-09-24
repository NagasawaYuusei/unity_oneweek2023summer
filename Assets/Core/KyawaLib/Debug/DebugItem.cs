#if UNITY_EDITOR
using UnityEditor;

namespace KyawaLib
{
    public class DebugItem : ScriptableSingleton<DebugItem>
    {
        /// <summary>
        /// ゲーム中のデバッグボタンを表示するか（ゲーム画面を開く前のシーンで設定）
        /// </summary>
        public bool onIngameDebugButton { get; set; } = true;
    }
}
#endif