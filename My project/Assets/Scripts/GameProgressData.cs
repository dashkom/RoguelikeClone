using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class GameProgressData
{
    [Serializable]
    public class Records
    {
        public int Level { get; set; }

        public Records(int level)
        {
            Level = level;
        }
        public Records() { }

        public static List<Records> GetRecords (string path)
        {
            List<Records> records = new List<Records>();
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.OpenOrCreate)))
            {
                while (true)
                {
                    try
                    {
                        int level = reader.ReadInt32();
                        records.Add(new Records(level));
                    }
                    catch (EndOfStreamException) { break; }
                }
            }
            return records;
        }

        public static void SaveRecords (List<Records> records, string path)
        {
            using (BinaryWriter writer =  new BinaryWriter(File.Open(path,FileMode.OpenOrCreate)))
            {
                foreach (Records record in records)
                {
                    writer.Write(record.Level);
                }
            }
        }
        public static void AddRecords(Records records, string path) 
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
            {
                writer.Write(records.Level);
            }
        }
        public static void ClearRecords(string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Truncate)))
            {
            }
        }
    }
}