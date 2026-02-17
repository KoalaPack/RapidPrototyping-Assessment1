using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public string[] checkTags;
    public UnityEvent onTriggerEnterEvent;
    public UnityEvent onTriggerExitEvent;
    public UnityEvent onTriggerStayEvent;

    public void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < checkTags.Length; i++)
        {
            if (other.gameObject.CompareTag(checkTags[i]))
                onTriggerEnterEvent?.Invoke();
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < checkTags.Length; i++)
        {
            if (other.gameObject.CompareTag(checkTags[i]))
                onTriggerExitEvent?.Invoke();
        }
    }

}
