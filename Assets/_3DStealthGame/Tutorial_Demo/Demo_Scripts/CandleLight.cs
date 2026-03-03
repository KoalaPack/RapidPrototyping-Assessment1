using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CandleLight : MonoBehaviour
{
    public BV.Range LightRange;     // Min/Max light radius
    public BV.Range TimeRange;      // Min/Max candle lifetime

    public float currentTime;

    public Light CandLight;

    public InputAction SprintAction;
    public float sprintDecrease = 2f;   // Extra drain per second while sprinting

    private void OnEnable()
    {
        SprintAction.Enable();
    }

    private void OnDisable()
    {
        SprintAction.Disable();
    }

    private void Start()
    {
        currentTime = TimeRange.max;
    }

    private void Update()
    {
        if (currentTime <= 0f)
        {
            CandLight.range = LightRange.min;
            return;
        }

        // Base candle drain
        currentTime -= Time.deltaTime;

        // Extra drain while sprinting (hold-based)
        if (SprintAction.IsPressed())
        {
            currentTime -= sprintDecrease * Time.deltaTime;
        }

        // Clamp so we don’t go outside bounds
        currentTime = Mathf.Clamp(currentTime, TimeRange.min, TimeRange.max);

        // Map time to light radius
        float mappedRange = MathX.Map(
            currentTime,
            TimeRange.min,
            TimeRange.max,
            LightRange.min,
            LightRange.max
        );

        CandLight.range = mappedRange;
    }

    public void ResetCandle()
    {
        currentTime = TimeRange.max;
    }
}