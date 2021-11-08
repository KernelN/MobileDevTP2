using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform joystickHolder;
    [SerializeField] RectTransform joystick;

    #region Unity Events
    private void Start()
    {
        joystick = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        MoveStick(eventData.position);
    }
    #endregion Unity Events

    void MoveStick(Vector2 pointerPosition)
    {
        Vector2 newPosition;
        newPosition.x = GetStickPosX(pointerPosition.x - joystickHolder.position.x);
        newPosition.y = GetStickPosY(pointerPosition.y - joystickHolder.position.y);
        joystick.anchoredPosition = newPosition;
    }
    float GetStickPosX(float x)
    {
        if (Mathf.Abs(x) < joystickHolder.sizeDelta.x / 2)
        {
            return x;
        }

        if (x > joystickHolder.sizeDelta.x / 2)
        {
            return joystickHolder.sizeDelta.x / 2 - joystickHolder.sizeDelta.x / 10;
        }
        else
        {
            return -joystickHolder.sizeDelta.x / 2 + joystickHolder.sizeDelta.x / 10;
        }
    }
    float GetStickPosY(float y)
    {
        if (Mathf.Abs(y) < joystickHolder.sizeDelta.y / 2)
        {
            return y;
        }

        if (y > joystickHolder.sizeDelta.y / 2)
        {
            return joystickHolder.sizeDelta.y / 2 - joystickHolder.sizeDelta.x / 10;
        }
        else
        {
            return -joystickHolder.sizeDelta.y / 2 + joystickHolder.sizeDelta.x / 10;
        }
    }
}