using Sirenix.OdinInspector;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField, OnValueChanged(nameof(UpdateSkin), IncludeChildren = true, InvokeOnInitialize = true, InvokeOnUndoRedo = true)] private Skin _skin;
    [SerializeField, OnValueChanged(nameof(UpdateSkin), IncludeChildren = true, InvokeOnInitialize = true, InvokeOnUndoRedo = true)] private CardData _data;
    [SerializeField] private List<SpriteRenderer> _renderers;
    private void UpdateSkin()
    {
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
    }
}
