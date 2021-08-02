#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public static class CSVLoader
    {
        private const char lineSperator = '\n';
        private const char surround = '"';
        private static string fieldSperator = "\",\"";
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public static string SerializeTableLine(string[] _fields)
        {

            for (int f = 0; f < _fields.Length; f++)
            {
                if (string.IsNullOrEmpty(_fields[f]))
                    _fields[f] = "";
                else
                    _fields[f] = _fields[f].Replace("\"", "\"\"");
            }
            return string.Concat("\"", string.Join(fieldSperator, _fields), "\"");
        }

        public static string SerializeTable(string[][] _dataTable)
        {
            StringBuilder sb = new StringBuilder();
            for (int lineIndex = 0; lineIndex < _dataTable.Length; lineIndex++)
            {
                sb.AppendLine(SerializeTableLine(_dataTable[lineIndex]));
            }
            return sb.ToString();
        }

        public static string[] DeserializeTableLine(string line)
        {
            string[] fields = CSVParser.Split(line);
            for (int f = 0; f < fields.Length; f++)
            {
                if (fields[f].Contains(","))
                {
                    fields[f] = fields[f].Substring(1);
                    fields[f] = fields[f].Remove(fields[f].LastIndexOf("\""));
                }
                fields[f] = fields[f].Replace("\"\"", "\"");
            }
            return fields;
        }

        public static void DeserializeTable(string text, Action<string[]> eachLineCallback)
        {
            string[] lines = text.Split(lineSperator);
            string[][] dataTable = new string[lines.Length][];
            bool callback = eachLineCallback != null;
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;
                string[] fields = DeserializeTableLine(lines[i]);
                eachLineCallback(fields);
                dataTable[i] = fields;
            }
        }
    }

    public class Localization
    {
        int language;
        Dictionary<string, string[]> texts = new Dictionary<string, string[]>();

        public event Action onLanguageChanged;

        public string[] Languages
        {
            get { return texts["Key"]; }
        }
        public int Language
        {
            get { return language; }
            set
            {
                if (language == value) return;
                language = value;
                onLanguageChanged?.Invoke();
            }
        }

        public Localization(string _text)
        {
            CSVLoader.DeserializeTable(_text, _ =>
            {
                texts[_[0]] = _.Skip(1).ToArray();
            });
        }

        public string GetText(string _key)
        {
            if (texts.TryGetValue(_key, out string[] _texts))
            {
                if (_texts.Length > language)
                    return _texts[language];
            }
            return _key;
        }
        Dictionary<string, GUIContent_Extend> contents = new Dictionary<string, GUIContent_Extend>();
        public GUIContent GetGUIContent(string _key)
        {
            if (contents.TryGetValue(_key, out GUIContent_Extend content))
                return content;
            return contents[_key] = content = new GUIContent_Extend(_key, this);
        }
    }

    public class GUIContent_Extend : GUIContent
    {
        string key;
        Localization owner;

        public string Key
        {
            get { return key; }
            set
            {
                if (key == value) return;
                key = value;
                text = owner.GetText(key);
            }
        }

        public GUIContent_Extend(Localization _owner)
        {
            owner = _owner;
            owner.onLanguageChanged += Refresh;
        }

        public GUIContent_Extend(string _key, Localization _owner) : base(_owner.GetText(_key))
        {
            key = _key;
            owner = _owner;
            owner.onLanguageChanged += Refresh;
        }

        public GUIContent_Extend(Texture image, Localization _owner) : base(image)
        {
            owner = _owner;
            owner.onLanguageChanged += Refresh;
        }

        public GUIContent_Extend(string _key, Texture image, Localization _owner) : base(_owner.GetText(_key), image)
        {
            key = _key;
            owner = _owner;
            owner.onLanguageChanged += Refresh;
        }

        void Refresh()
        {
            text = owner.GetText(key);
        }
    }
}
