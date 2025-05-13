using UnityEngine;

public class Swipe : MonoBehaviour
{
  // Input Manager Reference
  private InputManager inputManager;

  private PlayerSystem playerSystem;

  // Swipe Configuration
  [SerializeField]
  private float minimumDistance = .2f; // Minimum distance required to register a swipe
  [SerializeField]
  private float maximumTime = .5f; // Maximum time allowed for a swipe
  [SerializeField, Range(0f, 1f)]
  private float directionThreshold = .9f; // Threshold to determine swipe direction

  // Swipe Data
  private Vector2 startPosition; // Starting position of the swipe
  private float startTime; // Starting time of the swipe
  private Vector2 endPosition; // Ending position of the swipe
  private float endTime; // Ending time of the swipe


  // Unity Lifecycle Methods
  private void Awake()
  {
    inputManager = InputManager.Instance; // Get the instance of InputManager
    playerSystem = PlayerSystem.Instance;
  }

  private void OnEnable()
  {
    inputManager.OnStartTouch += SwipeStart; // Subscribe to the start touch event
    inputManager.OnEndTouch += SwipeEnd; // Subscribe to the end touch event
  }

  private void OnDisable()
  {
    inputManager.OnStartTouch -= SwipeStart; // Unsubscribe from the start touch event
    inputManager.OnEndTouch -= SwipeEnd; // Unsubscribe from the end touch event
  }

  // Swipe Event Handlers
  private void SwipeStart(Vector2 position, float time)
  {
    startPosition = position; // Record the starting position of the swipe
    startTime = time; // Record the starting time of the swipe
  }

  private void SwipeEnd(Vector2 position, float time)
  {
    endPosition = position; // Record the ending position of the swipe
    endTime = time; // Record the ending time of the swipe
    DetectSwipe(); // Detect if the input qualifies as a swipe
  }

  // Swipe Detection Logic
  private void DetectSwipe()
  {
    // Check if the swipe meets the minimum distance and maximum time criteria
    if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
    (endTime - startTime) <= maximumTime)
    {
      Debug.Log("SwipeDetection");
      Debug.DrawLine(startPosition, endPosition, Color.white, 5f); // Draw a debug line for the swipe
      Vector3 direction = endPosition - startPosition; // Calculate the swipe direction in 3D
      Vector2 direction2D = endPosition - startPosition; // Calculate the swipe direction in 2D
      SwipeDirection(direction2D); // Determine the swipe direction
    }
  }

  // Swipe Direction Handling
  private void SwipeDirection(Vector2 direction)
  {
    // Check if the swipe direction is upward
    if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
    {
      Debug.Log("Swipe Up");
      playerSystem.Jump(); // Trigger the jump action
    }
    // Check if the swipe direction is downward
    else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
    {
      Debug.Log("Swipe Down"); 
      playerSystem.Slide(); // Trigger the slide action
    }
  }
}
