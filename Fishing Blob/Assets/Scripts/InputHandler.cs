using UnityEngine; 
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] 
    private PlayerInput playerInput;

    private bool _cast;
    private bool _reelTriggered;

    private InputAction _castAction;
    private InputAction _reelAction;
    
    private void Awake()
    {
        _castAction = playerInput.actions["Cast"];
        _reelAction = playerInput.actions["Reel"];
        
        _reelAction.performed += OnReelPerformed;
    }

    private void OnDestroy()
    {
        _reelAction.performed -= OnReelPerformed;
    }

    private void Update()
    {
        _cast = _castAction.IsPressed();
    }

    private void OnReelPerformed(InputAction.CallbackContext context)
    {
        _reelTriggered = true;
    }

    private void LateUpdate()
    {
        _reelTriggered = false;
    }
    

    public bool GetCastValue()
    {
        return _cast;
    }

    public bool ReelPerformed()
    {
        return _reelTriggered;
    }
    
}
 