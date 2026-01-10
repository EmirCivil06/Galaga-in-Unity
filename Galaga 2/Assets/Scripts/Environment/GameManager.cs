using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputActionReference pause;
    public Stack<int> pauseController = new Stack<int>(1);
    public bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (pause.action.WasPressedThisFrame())
        {
            if (pauseController.Count == 0)
            {
                PushPause();
            } 
            else 
            {
                PopPause();
            }
        }
    }

    public void PushPause()
    {
        pauseController.Push(1);
        Debug.Log("Pushed");
        UpdateTimeScale();
    }

    public void PopPause()
    {
        if (pauseController.Count > 0)
        {
            pauseController.Pop();
        } 
        Debug.Log("Popped");
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {
        Time.timeScale = pauseController.Count > 0f? 0f : 1f;
        // Debug.Log($"Stackde yer alan değer: {pauseController.Count} | Zaman Skalası: {Time.timeScale}");
        isPaused = pauseController.Count > 0f? true : false;
    }
    
}
