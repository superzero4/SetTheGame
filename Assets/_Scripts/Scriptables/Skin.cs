using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "New skin", menuName = "Scriptables/Skin")]
public class Skin : ScriptableObject
{
    [SerializeField] private List<int> _count;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private List<Material> _materials;
    [SerializeField] private List<Sprite> _sprites;
    public Result this[CardData indexer] => (indexer.Count, _colors[indexer.Color], _materials[indexer.Fill], _sprites[indexer.Shape]);
    public IList this[DataType t]
    {
        get
        {
            switch (t)
            {
                case DataType.Count:
                    return _count;
                case DataType.Color:
                    return _colors;
                case DataType.Fill:
                    return _materials;
                case DataType.Shape:
                    return _sprites;
                default:
                    return null;
            }
        }
    }

    private bool IsValid()
    {
        return  _materials.Count == EnumHelpers.Count<Fill>()
            && _colors.Count == EnumHelpers.Count<Colors>()
            && _sprites.Count == EnumHelpers.Count<Shape>()
            && _count.Count == EnumHelpers.Count<Count>();
    }
    public static bool ValidateSkin(Skin skin) => skin != null && skin.IsValid();
#if UNITY_EDITOR

    private const string BaseSkinFileName = "BaseSkin";
    public static Skin LoadDefaultSkin()
    {
        var skins = AssetDatabase.FindAssets("t:" + nameof(Skin))
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<Skin>(path));
        return skins.FirstOrDefault(s=>s.name.Equals(BaseSkinFileName)) ?? skins.First();
    }
#endif
}
