using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentBeforeDrag;
    public Transform parentAfterDrag;
    private Image image;
    private Text text;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        parentBeforeDrag = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = parentBeforeDrag;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        text.raycastTarget = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        // Move the item back to its parent after drag
        transform.SetParent(parentAfterDrag);
        transform.localRotation = Quaternion.identity;
        
        image.raycastTarget = true;
        text.raycastTarget = true;
    }
}
