using Sirenix.OdinInspector;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class GameLogic : MonoBehaviour
{
    //Temporary, we'll probably use some 2D structures later
    [SerializeField] private List<Card> _cards;
    private IEnumerable<Card> Cards => _cards;
    private void Update()
    {
        int i = 0;
        foreach (var card in Cards)
        {
            card.gameObject.name="Card "+i++;
            card.Data.Randomize();
            if (FindSet(out Set set))
                Debug.Log(set);
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
                    var counts = new int[] { card.Data.Count, card2.Data.Count, card2.Data.Count };
                    var Fills = new int[] { card.Data.Fill, card2.Data.Fill, card2.Data.Fill };
                    var Shapes = new int[] { card.Data.Shape, card2.Data.Shape, card2.Data.Shape };
                    var Colors = new int[] { card.Data.Color, card2.Data.Color, card2.Data.Color };
                    if (ValidateSequence(counts, counts[0]) && ValidateSequence(Fills, Fills[0]) && ValidateSequence(Shapes, Shapes[0]) && ValidateSequence(Colors, Colors[0]))
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
