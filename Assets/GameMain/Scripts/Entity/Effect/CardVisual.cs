using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CCx
{
    public class CardVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Canvas canvas;
        private Image imageComponent;
        [SerializeField] private bool instantiateVisual = true;
        private VisualCardsHandler visualHandler;
        private Vector3 offset;

        [Header("Movement")]
        [SerializeField] private float moveSpeedLimit = 50;

        [Header("Selection")]
        public bool selected;
        public float selectionOffset = 50;
        private float pointerDownTime;
        private float pointerUpTime;

        [Header("Visual")]
        [SerializeField] private GameObject cardVisualPrefab;
        [HideInInspector] public CardVisualItem cardVisual;

        [Header("States")]
        public bool isHovering;
        public bool isDragging;
        [HideInInspector] public bool wasDragged;

        [Header("Events")]
        [HideInInspector] public UnityEvent<CardVisual> PointerEnterEvent;
        [HideInInspector] public UnityEvent<CardVisual> PointerExitEvent;
        [HideInInspector] public UnityEvent<CardVisual, bool> PointerUpEvent;
        [HideInInspector] public UnityEvent<CardVisual> PointerDownEvent;
        [HideInInspector] public UnityEvent<CardVisual> BeginDragEvent;
        [HideInInspector] public UnityEvent<CardVisual> EndDragEvent;
        [HideInInspector] public UnityEvent<CardVisual, bool> SelectEvent;

        private Entity m_Owner = null;
        private int m_OwnerId = 0;
        float currentPosY;
        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }
        public void Init(Entity owner, Canvas _canvas)
        {
            canvas = _canvas;
            imageComponent = GetComponent<Image>();

            if (!instantiateVisual)
                return;

            visualHandler = FindObjectOfType<VisualCardsHandler>();
            if (cardVisual == null)
            {
                cardVisual = Instantiate(cardVisualPrefab, visualHandler.transform).GetComponent<CardVisualItem>();
                cardVisual.Initialize(this);
            }
            cardVisual.gameObject.SetActive(true);
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                m_Owner = owner;
                m_OwnerId = owner.Id;
            }
        }
        public void Reset()
        {
            m_Owner = null;
            m_OwnerId = -1;
            PointerEnterEvent.RemoveAllListeners();
            PointerExitEvent.RemoveAllListeners();
            BeginDragEvent.RemoveAllListeners();
            EndDragEvent.RemoveAllListeners();
            transform.localPosition = Vector3.zero;
            cardVisual.Reset();
            this.transform.parent.gameObject.SetActive(false);
        }

        public void OnUpdate()
        {
            ClampPosition();
            if (cardVisual != null && cardVisual.gameObject.activeSelf)
            {
                cardVisual.OnUpdate();
            }
            if (isDragging)
            {
                Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
                transform.Translate(velocity * Time.deltaTime);
            }
        }

        void ClampPosition()
        {
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent.Invoke(this);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = mousePosition - (Vector2)transform.position;
            isDragging = true;
            canvas.GetComponent<GraphicRaycaster>().enabled = false;
            imageComponent.raycastTarget = false;
            wasDragged = true;
            currentPosY = transform.localPosition.y;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvas.GetComponent<GraphicRaycaster>().enabled = true;
            EndDragEvent.Invoke(this);
            isDragging = false;
            imageComponent.raycastTarget = true;

            StartCoroutine(FrameWait());

            IEnumerator FrameWait()
            {
                yield return new WaitForEndOfFrame();
                wasDragged = false;
            }
            if (Mathf.Abs(currentPosY - transform.localPosition.y) > 200)
            {
                GameEntry.SkillSlot.UseSkillCard(this);
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterEvent.Invoke(this);
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExitEvent.Invoke(this);
            isHovering = false;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            //if (eventData.button != PointerEventData.InputButton.Left)
            //    return;

            //PointerDownEvent.Invoke(this);
            //pointerDownTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //if (eventData.button != PointerEventData.InputButton.Left)
            //    return;

            //pointerUpTime = Time.time;

            //PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);

            //if (pointerUpTime - pointerDownTime > .2f)
            //    return;

            //if (wasDragged)
            //    return;

            //selected = !selected;
            //SelectEvent.Invoke(this, selected);

            //if (selected)
            //    transform.localPosition += (cardVisual.transform.up * selectionOffset);
            //else
            //    transform.localPosition = Vector3.zero;
        }

        public void Deselect()
        {
            if (selected)
            {
                selected = false;
                if (selected)
                    transform.localPosition += (cardVisual.transform.up * 50);
                else
                    transform.localPosition = Vector3.zero;
            }
        }


        public int SiblingAmount()
        {
            return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
        }

        public int ParentIndex()
        {
            return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        }

        public float NormalizedPosition()
        {
            return transform.parent.CompareTag("Slot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
        }

        private void OnDestroy()
        {
            if (cardVisual != null)
                Destroy(cardVisual.gameObject);
        }
    }
}

