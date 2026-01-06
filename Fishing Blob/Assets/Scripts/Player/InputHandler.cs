using UnityEngine; 
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] 
    private PlayerInput playerInput;
    
    [SerializeField]
    private string castActionName = "Cast";
    [SerializeField]
    private string reelActionName = "Reel";
    [SerializeField]
    private string moveActionName = "Move";
    [SerializeField]
    private string pauseActionName = "Pause";
    [SerializeField]
    private string interactActionName = "Interact";

    private InputAction _castAction;
    private InputAction _reelAction;
    private InputAction _moveAction;
    private InputAction _pauseAction;
    private InputAction _interactAction;
    
    private bool _cast;
    private bool _reelTriggered;
    private bool _pauseTriggered;
    private bool _interactTriggered;
    
    private void Awake()
    {
        _castAction = playerInput.actions.FindAction(castActionName);
        _reelAction = playerInput.actions.FindAction(reelActionName);
        _moveAction =  playerInput.actions.FindAction(moveActionName);
        _pauseAction = playerInput.actions.FindAction(pauseActionName);
        _interactAction = playerInput.actions.FindAction(interactActionName);
    }
    
    private void Update()
    {
        _cast = _castAction.IsPressed();
        _reelTriggered = _reelAction.IsPressed();
        _pauseTriggered = _pauseAction.WasPressedThisFrame();
        _interactTriggered = _interactAction.WasPressedThisFrame();
    }
    public bool GetCastValue() => _cast;
    public bool GetReelValue() => _reelTriggered;
    public bool GetPauseValue() => _pauseTriggered;
    public bool GetInteractValue() => _interactTriggered;
    public Vector2 GetMoveValue() => _moveAction.ReadValue<Vector2>();
    
}
 