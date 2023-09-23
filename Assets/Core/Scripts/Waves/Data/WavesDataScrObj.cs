using UnityEngine;
using System;

/// <summary>
/// ゲーム内Waveデータ
/// </summary>
namespace Data
{
    [CreateAssetMenu(fileName = "WavesData", menuName = "Project/WavesData")]
    public class WavesDataScrObj : ScriptableObject
    {
        [SerializeField]
        private WaveData[] _waveData = null;

        [SerializeField]
        private string _bossName = string.Empty;

        [SerializeField, ReadOnly]
        private int _bossGenerateCount = 1; // ボスは必ず1体

        /// <summary>
        /// ボス前のWaveデータ
        /// </summary>
        public WaveData[] waveData => _waveData;

        /// <summary>
        /// ボスのWaveデータ
        /// </summary>
        public WaveData bossWaveData { get; private set; } = null;

        /// <summary>
        /// 各Waveでスポーンする敵の種類と数
        /// </summary>
        [Serializable]
        public class WaveData
        {
            [SerializeField]
            private SpawnRatioData[] _spawnRatioData;
            public SpawnRatioData[] spawnRatioData => _spawnRatioData;

            [SerializeField, Range(1, 20)]
            private int _generateCount = 0;
            public int generateCount => _generateCount;

            public WaveData(SpawnRatioData[] spawnRatioData, int generateCount)
            {
                _spawnRatioData = spawnRatioData;
                _generateCount = generateCount;
            }
        }

        /// <summary>
        /// スポーン情報
        /// </summary>
        [Serializable]
        public class SpawnRatioData
        {
            [SerializeField]
            private string _name;
            public string name => _name;

            [SerializeField, Min(0)]
            private int _ratio;
            public int ratio => _ratio;

            public SpawnRatioData(string name, int ratio)
            {
                _name = name;
                _ratio = ratio;
            }
        }

        void OnEnable()
        {
            // ボスのWaveDataを作成
            bossWaveData = new WaveData(
                new SpawnRatioData[] { new SpawnRatioData(_bossName, 1) },
                _bossGenerateCount);
        }
    }
}