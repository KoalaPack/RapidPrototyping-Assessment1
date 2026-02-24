using UnityEngine;

public class CandleLight: MonoBehaviour
{
    public BV.Range LightRange;
    public BV.Range TimeRange;

    public float currentTime;
    public Timer timer;

    public Light CandLight;

    public void Update()
    {
        currentTime -= Time.deltaTime;

        float temp = MathX.Map(currentTime, TimeRange.min, TimeRange.max, LightRange.min, LightRange.max);
        CandLight.range = temp;


    }

    public void ResetCandle()
    {
        currentTime = TimeRange.max;
    }
}
