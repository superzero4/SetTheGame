using Sirenix.OdinInspector;
using Structures;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private const string ValidateCurrentSkinInvoke =
        "@" + nameof(Skin) + "." + nameof(Skin.ValidateSkin) + "(" + nameof(_skin) + ")";

    [SerializeField,
     OnValueChanged(nameof(UpdateSkin), IncludeChildren = true, InvokeOnInitialize = true, InvokeOnUndoRedo = true),
     ValidateInput(nameof(ValidateSkinHelper), "Skin isn't valid, check the scriptable for detailed info"),
     InlineEditor]
    private Skin _skin;

    [SerializeField,
     OnValueChanged(nameof(UpdateSkin), IncludeChildren = true, InvokeOnInitialize = true, InvokeOnUndoRedo = true),
     ShowIf(ValidateCurrentSkinInvoke)]
    private CardData _data;

    [SerializeField] private List<SpriteRenderer> _renderers;
    [SerializeField] private SpriteRenderer _highlightRenderer;

    public CardData Data
    {
        get => _data;
    }

    public void Randomize(IEnumerable<Card> toAvoid)
    {
        do
        {
            _data.Randomize();
        } while (toAvoid.Any(c => c.Data.Equals(_data)));
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
    Coroutine _routine;

    public void Highlight(Color highlightColor, float duration = 1f)
    {
        _highlightRenderer.color = highlightColor;
        if (_routine != null)
            StopCoroutine(_routine);
        _routine = StartCoroutine(HighlightRoutine(duration));
    }

    private IEnumerator HighlightRoutine(float duration = 1f)
    {
        _highlightRenderer.gameObject.SetActive(true);
        if (duration > 0f)
        {
            yield return new WaitForSeconds(duration);
            _highlightRenderer.gameObject.SetActive(false);
        }
    }

    public void HideHighlight()
    {
        _highlightRenderer.gameObject.SetActive(false);
    }
}