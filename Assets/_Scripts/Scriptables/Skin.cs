using Sirenix.OdinInspector;
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
    [InfoBox("@" + nameof(EnumList) + "<" + nameof(Count) + ">()")]
    [SerializeField, ValidateInput("@" + nameof(ListMatchesSize) + "<" + nameof(Count) + ">(" + nameof(_count) + ")", "@" + nameof(ErrorMessage) + "<" + nameof(Count) + ">(" + nameof(_count) + ")")] private List<int> _count;
    [InfoBox("@" + nameof(EnumList) + "<" + nameof(Colors) + ">()")]
    [SerializeField, ValidateInput("@" + nameof(ListMatchesSize) + "<" + nameof(Colors) + ">(" + nameof(_colors) + ")", "@" + nameof(ErrorMessage) + "<" + nameof(Colors) + ">(" + nameof(_colors) + ")")] private List<Color> _colors;
    [InfoBox("@" + nameof(EnumList) + "<" + nameof(Fill) + ">()")]
    [SerializeField, ValidateInput("@" + nameof(ListMatchesSize) + "<" + nameof(Fill) + ">(" + nameof(_materials) + ")", "@" + nameof(ErrorMessage) + "<" + nameof(Fill) + ">(" + nameof(_materials) + ")")] private List<Material> _materials;
    [InfoBox("@" + nameof(EnumList) + "<" + nameof(Shape) + ">()")]
    [SerializeField, ValidateInput("@" + nameof(ListMatchesSize) + "<" + nameof(Shape) + ">(" + nameof(_sprites) + ")", "@" + nameof(ErrorMessage) + "<" + nameof(Shape) + ">(" + nameof(_sprites) + ")")] private List<Sprite> _sprites;
    public Result this[CardData indexer] => (_count[indexer.Count], _colors[indexer.Color], _materials[indexer.Fill], _sprites[indexer.Shape]);
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
        return ListMatchesSize<Fill>(_materials)
            && ListMatchesSize<Colors>(_colors)
            && ListMatchesSize<Shape>(_sprites)
            && ListMatchesSize<Count>(_count);
    }
    private bool ListMatchesSize<T>(IList list) where T : Enum => list.Count == EnumHelpers.Count<T>();
    private string ErrorMessage<T>(IList list) where T : Enum => "There is " + list.Count + " instead of " + EnumHelpers.Count<T>() + " values";
    private string EnumList<T>() where T : Enum => "Member IN ORDER are : " + string.Join(", ", EnumHelpers.Values<T>().Select(e => e.ToString()));
    public static bool ValidateSkin(Skin skin) => skin != null && skin.IsValid();
#if UNITY_EDITOR

    private const string BaseSkinFileName = "BaseSkin";
    public static Skin LoadDefaultSkin()
    {
        var skins = AssetDatabase.FindAssets("t:" + nameof(Skin))
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<Skin>(path));
        return skins.FirstOrDefault(s => s.name.Equals(BaseSkinFileName)) ?? skins.First();
    }
#endif
}
