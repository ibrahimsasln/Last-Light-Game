using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Tilemania.Managers
{
    public class HighScoreTable
    {
        const string SaveKey = "HighScores";
        const int MaxEntries = 10;

        [Serializable]
        class SaveData
        {
            public List<HighScoreEntry> Entries = new List<HighScoreEntry>();
        }

        readonly SaveData data;

        public IReadOnlyList<HighScoreEntry> Entries => data.Entries;
        public int BestWave => data.Entries.Count > 0 ? data.Entries[0].Wave : 0;

        public HighScoreTable()
        {
            string json = PlayerPrefs.GetString(SaveKey, string.Empty);
            data = string.IsNullOrEmpty(json) ? new SaveData() : JsonUtility.FromJson<SaveData>(json);
        }

        public int Add(int wave)
        {
            if (wave <= 0) return -1;

            HighScoreEntry entry = new HighScoreEntry
            {
                Wave = wave,
                Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            };

            int rank = data.Entries.FindIndex(existing => wave > existing.Wave);
            if (rank < 0)
            {
                rank = data.Entries.Count;
            }
            if (rank >= MaxEntries) return -1;

            data.Entries.Insert(rank, entry);
            if (data.Entries.Count > MaxEntries)
            {
                data.Entries.RemoveAt(data.Entries.Count - 1);
            }

            Save();
            return rank;
        }

        private void Save()
        {
            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }
}
