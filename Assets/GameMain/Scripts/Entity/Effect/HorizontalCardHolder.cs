using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
namespace CCx
{
    public class HorizontalCardHolder : MonoBehaviour
    {
        public static HorizontalCardHolder instance;
        [SerializeField] private CardVisual selectedCard;
        [SerializeReference] private CardVisual hoveredCard;

        [SerializeField] private GameObject slotPrefab;
        private RectTransform rect;

        [Header("Spawn Settings")]
        [SerializeField] private int cardsToSpawn = 7;
        public List<CardVisual> cards;

        bool isCrossing = false;
        [SerializeField] private bool tweenCardReturn = true;

        private void Awake()
        {
            instance = this;
        }
        int cardCount = 0;
        void Start()
        {
            rect = GetComponent<RectTransform>();
            StartCoroutine(Frame());

            IEnumerator Frame()
            {
                yield return new WaitForSecondsRealtime(.1f);
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].cardVisual != null)
                        cards[i].cardVisual.UpdateIndex(transform.childCount);
                }
            }
        }

        private void BeginDrag(CardVisual card)
        {
            selectedCard = card;
        }

        public void AddCard(CardVisual card)
        {
            cards.Add(card);
            card.PointerEnterEvent.AddListener(CardPointerEnter);
            card.PointerExitEvent.AddListener(CardPointerExit);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);
            card.name = cardCount.ToString();
            cardCount++;
        }

        void EndDrag(CardVisual card)
        {
            if (selectedCard == null)
                return;

            selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0, selectedCard.selectionOffset, 0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

            rect.sizeDelta += Vector2.right;
            rect.sizeDelta -= Vector2.right;

            selectedCard = null;

        }

        void CardPointerEnter(CardVisual card)
        {
            hoveredCard = card;
        }

        void CardPointerExit(CardVisual card)
        {
            hoveredCard = null;
        }

        void Swap(int index)
        {
            isCrossing = true;

            Transform focusedParent = selectedCard.transform.parent;
            Transform crossedParent = cards[index].transform.parent;

            cards[index].transform.SetParent(focusedParent);
            cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
            selectedCard.transform.SetParent(crossedParent);

            isCrossing = false;

            if (cards[index].cardVisual == null)
                return;

            bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
            cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

            //Updated Visual Indexes
            foreach (var card in cards)
            {
                card.cardVisual.UpdateIndex(transform.childCount);
            }
        }

    }
}

