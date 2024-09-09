using Sirenix.OdinInspector;
using Structures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    //Temporary, we'll probably use some 2D structures later
    [SerializeField, Range(-1f, 10f)] private float _delay;
    [SerializeField] private List<Card> _cards;
    private IEnumerable<Card> Cards => _cards;

    private IEnumerator Start()
    {
        List<Card> randomizedCards = new List<Card>();
        while (true)
        {
            int i = 0;
            randomizedCards.Clear();
            foreach (var card in Cards)
            {
                card.gameObject.name = "Card " + i++;
                card.Randomize(randomizedCards);
                randomizedCards.Add(card);
            }

            if (FindSet(out Set set))
            {
                Debug.Log(set);
                Color highlightColor = Color.green;
                set.Item1.Highlight(highlightColor,_delay);
                set.Item2.Highlight(highlightColor,_delay);
                set.Item3.Highlight(highlightColor,_delay);
            }
            else
                Debug.LogWarning("No set founds");

            if (_delay > 0f)
                yield return new WaitForSeconds(_delay);
            else
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                if (set != default)
                { 
                    set.Item1.HideHighlight();
                    set.Item2.HideHighlight();
                    set.Item3.HideHighlight();   
                }
            }
        }
    }

    [Button(nameof(GetAllChilds))]
    private void GetAllChilds()
    {
        _cards = GetComponentsInChildren<Card>().ToList();
    }

    private bool FindSet(out Set set)
    {
        set = null;
        foreach (var card in _cards)
        {
            foreach (var card2 in _cards.Where(c => c != card))
            {
                foreach (var card3 in _cards.Where(c => c != card && card != card2))
                {
                    var counts = new int[] { card.Data.Count, card2.Data.Count, card3.Data.Count };
                    var Fills = new int[] { card.Data.Fill, card2.Data.Fill, card3.Data.Fill };
                    var Shapes = new int[] { card.Data.Shape, card2.Data.Shape, card3.Data.Shape };
                    var Colors = new int[] { card.Data.Color, card2.Data.Color, card3.Data.Color };
                    if (ValidateSequence(counts, counts[0]) && ValidateSequence(Fills, Fills[0]) &&
                        ValidateSequence(Shapes, Shapes[0]) && ValidateSequence(Colors, Colors[0]))
                    {
                        set = (card, card2, card3);
                        return true;
                    }
                }
            }
        }

        return false;
    }


    private bool ValidateSequence(IEnumerable<int> values, int oneElement)
    {
        return values.All(c => c == oneElement) || values.Distinct().Count() == EnumHelpers.Count<Count>();
    }
}