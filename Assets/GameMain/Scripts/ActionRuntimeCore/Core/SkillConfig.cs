using System;
using System.IO;
using FullSerializer;
using UnityEditor;
using UnityEngine;
namespace CCx
{
    public class SkillConfig 
    {

        public int Id;

        public int Atk;
        public int Def;

        /// <summary>
        /// 时间轴配置名
        /// </summary>
        public string EventName;


        private SkillAsset _skillAsset;

        public SkillAsset EventConfig
        {
            get
            {
                if (_skillAsset == null)
                {
#if UNITY_EDITOR
                    var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/{EventName}.json");
                    if (textAsset != null)
                    {
                        _skillAsset = Json.Deserialize(typeof(SkillAsset), textAsset.text) as SkillAsset;
                    }
                    //演示直接使用editor的资源方法
                    // _skillAsset =
                    //     UnityEditor.AssetDatabase.LoadAssetAtPath<SkillAsset>($"Assets/ResRaw/Skill/{EventName}.asset");
#endif
                }

                return _skillAsset;
            }
        }
    }

    public class Json
    {
        public static string Serialize(object value, bool isCompressed = false)
        {
            try
            {
                new fsSerializer().TrySerialize(value, out var data).AssertSuccessWithoutWarnings();
                if (isCompressed)
                {
                    return fsJsonPrinter.CompressedJson(data);
                }

                return fsJsonPrinter.PrettyJson(data);
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static object Deserialize(Type type, string serializedState)
        {
            try
            {
                fsData data = fsJsonParser.Parse(serializedState);
                object deserialized = null;
                var ser = new fsSerializer();
                ser.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

                return deserialized;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                File.WriteAllText($"{Application.dataPath}/../error_json.json", serializedState);
#endif
                Debug.LogError(e);
                return null;
            }
        }
    }

}
