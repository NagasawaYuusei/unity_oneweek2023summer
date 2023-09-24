using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KyawaLib
{
    public class EditorMenuItem : MonoBehaviour
    {
        /// <summary>
        /// ゲーム画面のスクリーンショットを保存する
        /// </summary>
        [MenuItem("KyawaLib/Capture Screenshot &S")]
        static void TakeScreenShot()
        {
            var folder = String.Concat(Application.dataPath.Replace("Assets", "../"), "ScreenShots/");
            var file = DateTime.Now.ToString("yyMMdd-HHmmss");
            var path = $"{folder}{file}.png";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"Captured Screenshot. : {path}");
        }

        /// <summary>
        /// ゲーム中のデバッグボタンを表示するか（ゲーム画面を開く前のシーンで設定）
        /// </summary>
        [MenuItem("KyawaLib/InGame Debug Button")]
        static void OnIngameDebugButton()
        {
            var menuPath = "KyawaLib/InGame Debug Button";
            DebugItem.instance.onIngameDebugButton = Menu.GetChecked(menuPath);
            Menu.SetChecked(menuPath, !DebugItem.instance.onIngameDebugButton);
        }
    }
}