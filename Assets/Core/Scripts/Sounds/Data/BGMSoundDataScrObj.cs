using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// BGMサウンドデータ
/// </summary>
namespace Data
{
    [CreateAssetMenu(fileName = "SoundData_BGM", menuName = "Project/SoundData/SoundData_BGM")]
    public class BGMSoundDataScrObj : SoundDataScrObj
    {
        [SerializeField]
        List<BGMSoundData> m_soundData = null;

        [Serializable]
        class BGMSoundData : SoundData
        {
            public SoundType.BGM BgmType;
        }

        public SoundData GetSoundData(SoundType.BGM bgmType)
        {
            return m_soundData?.Find(data => data.BgmType == bgmType);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(BGMSoundDataScrObj))]
        class BGMSoundDataScrObjEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                var tgt = target as BGMSoundDataScrObj;
                if (tgt != null)
                {
                    var soundData = tgt.m_soundData;
                    if (soundData != null)
                    {
                        var enumLength = SoundType.BGMCount;
                        var dataLength = soundData.Count;
                        if (enumLength != dataLength) // データ数確認
                        {
                            EditorGUILayout.HelpBox("リストのデータ数が一致しません.", MessageType.Warning);
                        }
                        else // 重複確認
                        {
                            HashSet<SoundType.BGM> hashSet = new HashSet<SoundType.BGM>();
                            soundData.ForEach(data => hashSet.Add(data.BgmType));
                            if (hashSet.Count < dataLength)
                            {
                                EditorGUILayout.HelpBox("Bgm Typeが重複しています.", MessageType.Warning);
                            }
                        }
                    }
                }
                base.OnInspectorGUI();
            }
        }
#endif
    }
}