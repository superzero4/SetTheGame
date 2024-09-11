using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Structures;
using UnityEngine;
using UnityEngine.Events;
using Structures;

public class SetFinder : MonoBehaviour
{
    [SerializeField, Range(-1f, .5f)] private float _delay;

    private UnityEvent<Set> _onSetFound = new();

    internal UnityEvent<Set> OnSetFound => _onSetFound;
    private Coroutine _routine;

    public void FindSet(List<Card> cards)
    {
        if (_routine != null)
            StopCoroutine(_routine);
        _routine = StartCoroutine(SetCoroutine(cards));
    }

    private IEnumerator SetCoroutine(List<Card> cards)
    {
        Set checkedSet = null;
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            for (int j = i + 1; j < cards.Count; j++)
            {
                Card card2 = cards[j];
                for (int k = j + 1; k < cards.Count; k++)
                {
                    Card card3 = cards[k];
                    var counts = new int[] { card.Data.Count, card2.Data.Count, card3.Data.Count };
                    var Fills = new int[] { card.Data.Fill, card2.Data.Fill, card3.Data.Fill };
                    var Shapes = new int[] { card.Data.Shape, card2.Data.Shape, card3.Data.Shape };
                    var Colors = new int[] { card.Data.Color, card2.Data.Color, card3.Data.Color };
                    if (checkedSet != null)
                        checkedSet.HideHighlight(true);
                    checkedSet = new Set(card, card2, card3);
                    checkedSet.Highlight(Color.red, true);
                    if (ValidateSequence(counts, counts[0]) && ValidateSequence(Fills, Fills[0]) &&
                        ValidateSequence(Shapes, Shapes[0]) && ValidateSequence(Colors, Colors[0]))
                    {
                        _onSetFound.Invoke(checkedSet);
                    }

                    if (_delay > 0f)
                        yield return new WaitForSeconds(_delay);
                }
            }
        }

        if (checkedSet != null)
            checkedSet.HideHighlight(true);
        yield return null;
    }


    private static bool ValidateSequence(IEnumerable<int> values, int oneElement)
    {
        return values.All(c => c == oneElement) || values.Distinct().Count() == EnumHelpers.Count<Count>();
    }
}