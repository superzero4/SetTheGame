using Sirenix.OdinInspector;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card : MonoBehaviour
{
    private const string ValidateCurrentSkinInvoke = "@" + nameof(Skin) + "." + nameof(Skin.ValidateSkin) + "(" + nameof(_skin) + ")";
    [SerializeField, OnValueChanged(nameof(UpdateSkin), IncludeChildren = true, InvokeOnInitialize = true, InvokeOnUndoRedo = true), ValidateInput(nameof(ValidateSkinHelper), "Skin isn't valid, check the scriptable for detailed info"), InlineEditor] private Skin _skin;
    [SerializeField, OnValueChanged(nameof(UpdateSkin), IncludeChildren = true, InvokeOnInitialize = true, InvokeOnUndoRedo = true), ShowIf(ValidateCurrentSkinInvoke)] private CardData _data;
    [SerializeField] private List<SpriteRenderer> _renderers;

    public CardData Data { get => _data; }
    public void Randomize()
    {
        _data.Randomize();
        UpdateSkin();
    }
    public bool UpdateSkin()
    {
        if (!Skin.ValidateSkin(_skin))
            return false;
        var current = _skin[_data];
        int i = 0;
        for (; i < current.nbOfShape; i++)
        {
            var rend = _renderers[i];
            rend.gameObject.SetActive(true);
            rend.color = current.colorOfShapes;
            rend.material = current.materialOfShapes;
            rend.sprite = current.shapeOfShapes;
        }
        for (; i < _renderers.Count; i++)
        {
            _renderers[i].gameObject.SetActive(false);
        }
        return true;
    }
    public override string ToString()
    {
        return gameObject.name + " card has data : " + _data.ToString();
    }
#if UNITY_EDITOR
    private bool ValidateSkinHelper(Skin skin)
    {
        if (skin == null)
            _skin = skin = Skin.LoadDefaultSkin();
        return Skin.ValidateSkin(skin);
    }
#endif
}
