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
    private string item1ActionName = "Item1";

    private InputAction _castAction;
    private InputAction _reelAction;
    private InputAction _manageItem1Action;
    private InputAction _moveAction;
    
    private bool _cast;
    private bool _reelTriggered;
    private bool _manageItem1;
    
    private void Awake()
    {
        _castAction = playerInput.actions.FindAction(castActionName);
        _reelAction = playerInput.actions.FindAction(reelActionName);
        _manageItem1Action = playerInput.actions.FindAction(item1ActionName);
        _moveAction =  playerInput.actions.FindAction(moveActionName);
        Debug.Log($"ReelAction enabled: {_reelAction.enabled}, bindings: {_reelAction.bindings.Count}");
        
    }
    
    private void Update()
    {
        _cast = _castAction.IsPressed();
        _reelTriggered = _reelAction.WasPerformedThisFrame();          
        _manageItem1 = _manageItem1Action.WasPerformedThisFrame();
        
    }
    public bool GetCastValue() => _cast;
    public bool GetReelValue() => _reelTriggered;
    public bool GetManageItem1Value() => _manageItem1;
    public Vector2 GetMoveValue() => _moveAction.ReadValue<Vector2>();
    
}
 