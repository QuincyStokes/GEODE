using UnityEngine;
using UnityEngine.EventSystems;

public static class UICursorDetector
{
    public static bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}