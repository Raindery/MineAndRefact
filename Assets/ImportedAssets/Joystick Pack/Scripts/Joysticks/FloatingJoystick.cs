using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{

    private bool _isJoystickActive;
    public bool IsJoystickActive
    {
        get
        {
            return _isJoystickActive;
        }

        private set
        {
            _isJoystickActive = value;
            background.gameObject.SetActive(_isJoystickActive);
        }
    }


    protected override void Start()
    {
        base.Start();
        IsJoystickActive = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        IsJoystickActive = true;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        IsJoystickActive = false;
        base.OnPointerUp(eventData);
    }
}