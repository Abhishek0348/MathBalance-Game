using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropScript : MonoBehaviour, IDropHandler
{
    public ControlGame controlGame;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        DraggableItem draggableItem = draggedObject.GetComponent<DraggableItem>();

        int index = transform.GetSiblingIndex();

        
            // Replacing the items from the drop zone
            if (transform.childCount > 0)
            {
                Transform existingItem = transform.GetChild(0);
                existingItem.SetParent(draggableItem.parentAfterDrag);
                existingItem.SetAsLastSibling();
            }

            draggableItem.parentAfterDrag = transform;

            // Placing the dragged object in the drop zone
            draggedObject.transform.position = transform.position;
            draggedObject.transform.SetParent(transform);
            draggedObject.transform.localRotation = Quaternion.identity;

            if (int.TryParse(draggedObject.GetComponentInChildren<Text>().text, out int number))
            {
                Debug.Log("Dropped number: " + number + " at index: " + index);
                controlGame.AddDroppedNumber(number, index);
            }
        }
        
    }
