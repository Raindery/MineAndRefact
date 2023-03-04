using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

[AddComponentMenu("Input/On-Screen Floating Joystick")]
[RequireComponent(typeof(FloatingJoystick))]
public class OnScreenFloatingJoystick : OnScreenControl, IPointerUpHandler, IDragHandler
{
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string _controlPath;


    private FloatingJoystick _floatingJoystick;


    protected override string controlPathInternal
    {
        get
        {
            return _controlPath;
        }

        set
        {
            _controlPath = value;
        }
    }

    private void OnValidate()
    {
        _floatingJoystick = GetComponent<FloatingJoystick>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        SendValueToControl(_floatingJoystick.Direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SendValueToControl(_floatingJoystick.Direction);
    }
}