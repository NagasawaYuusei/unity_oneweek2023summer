using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// SEサウンドデータ
/// </summary>
namespace Data
{
    [CreateAssetMenu(fileName = "SoundData_SE", menuName = "Project/SoundData/SoundData_SE")]
    public class SESoundDataScrObj : SoundDataScrObj
    {
        [SerializeField]
        List<SESoundData> m_soundData = null;

        [Serializable]
        class SESoundData : SoundData
        {
            public SoundType.SE SeType;
        }

        public SoundData GetSoundData(SoundType.SE seType)
        {
            return m_soundData?.Find(data => data.SeType == seType);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(SESoundDataScrObj))]
        class SESoundDataScrObjEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                var tgt = target as SESoundDataScrObj;
                if (tgt != null)
                {
                    var soundData = tgt.m_soundData;
                    if (soundData != null)
                    {
                        var enumLength = Enum.GetValues(typeof(SoundType.SE)).Length;
                        var dataLength = soundData.Count;
                        if (enumLength != dataLength) // データ数確認
                        {
                            EditorGUILayout.HelpBox("リストのデータ数が一致しません.", MessageType.Warning);
                        }
                        else // 重複確認
                        {
                            HashSet<SoundType.SE> hashSet = new HashSet<SoundType.SE>();
                            soundData.ForEach(data => hashSet.Add(data.SeType));
                            if (hashSet.Count < dataLength)
                            {
                                EditorGUILayout.HelpBox("Se Typeが重複しています.", MessageType.Warning);
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