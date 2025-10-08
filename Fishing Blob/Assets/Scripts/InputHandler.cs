using UnityEngine; 
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] 
    private PlayerInput playerInput;

    private bool _cast;

    private InputAction _castAction;

    private void Awake()
    {
        _castAction = playerInput.actions["Cast"];
    }

    private void Update()
    {
        _cast = _castAction.IsPressed();
    }

    public bool GetCastValue()
    {
        return _cast;
    }
}
