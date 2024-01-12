using UnityEngine;

public class URLOpener : MonoBehaviour
{
    public string href;

    public void OpenURL()
    {
        if (string.IsNullOrEmpty(href)) return;
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval($"window.open('{href}','_blank')");
#else
        Application.OpenURL(href);
#endif
    }
}