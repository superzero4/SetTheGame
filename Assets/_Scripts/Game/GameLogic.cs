using Sirenix.OdinInspector;
using Structures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private SetFinder _sets;

    //Temporary, we'll probably use some 2D structures later
    [SerializeField, Range(-1f, 10f)] private float _delay;
    [SerializeField, Range(-1f, 1f)] private float _findSetDelay = 0.01f;
    [SerializeField] private List<Card> _cards;
    private Set _lastSet;
    private IEnumerable<Card> Cards => _cards;

    private void OnSet(Set set)
    {
        this._lastSet = set;
        Debug.Log(set);
        Color highlightColor = set.GetHashColor();
        set.Highlight(highlightColor);
    }

    private IEnumerator Start()
    {
        _sets.OnSetFound.AddListener(OnSet);
        List<Card> randomizedCards = new List<Card>();
        for (int i=0;i<_cards.Count;i++)
            _cards[i].gameObject.name = "Card " + i;

        //Update
        while (true)
        {
            int i = 0;
            randomizedCards.Clear();
            foreach (var card in Cards)
            {
                card.Randomize(randomizedCards);
                randomizedCards.Add(card);
            }

            if (_lastSet != default)
                _lastSet.HideHighlight();

            _sets.FindSet(_cards);
            if (_delay > 0f)
                yield return new WaitForSeconds(_delay);
            else
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
    }

    [Button(nameof(GetAllChilds))]
    private void GetAllChilds()
    {
        _cards = GetComponentsInChildren<Card>().ToList();
    }
}