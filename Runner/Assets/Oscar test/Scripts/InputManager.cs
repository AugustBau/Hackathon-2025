using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)] // This means it runs before all other scripts
public class InputManager : Singleton<InputManager> 
{
    /// <summary>
    /// region events
    /// This region contains the events for the input system.
    /// The events are used to notify when a touch starts and ends.
    /// </summary>
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion

    private PlayerControls PlayerControls; // Ensure PlayerController is defined elsewhere in your project
    private Camera mainCamera;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Enable the input system when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }

    void Start()
    {
        PlayerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        PlayerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        // This method is triggered when the primary touch starts.
        // It invokes the OnStartTouch event, passing the touch position
        // converted to world coordinates and the time the touch started.
        if (OnStartTouch != null)
            OnStartTouch(Utils.ScreenToWorld(mainCamera, PlayerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        // This method is triggered when the primary touch ends.
        // It invokes the OnEndTouch event, passing the touch position
        // converted to world coordinates and the time the touch ended.
        if (OnEndTouch != null)
            OnEndTouch(Utils.ScreenToWorld(mainCamera, PlayerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
    }

    /// <summary>
    /// This method is used to retrieve the primary touch position in world coordinates.
    /// It utilizes the utility function to convert screen coordinates to world coordinates.
    /// </summary>
    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, PlayerControls.Touch.PrimaryPosition.ReadValue<Vector2>()); // Ensure Utile class and ScreenToWorld method are implemented
    }
}
