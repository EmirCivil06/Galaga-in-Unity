using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Oyunun bazı eylemlerin kontrol eden çekirdek script
    [SerializeField] private InputActionReference pause;
    public Stack<int> pauseController = new Stack<int>(1);
    public bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        // Eğer durdurma düğmesine basılırsa durduysa devam, devam ediyordu ise durma
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
    // Durdurma metodu
    public void PushPause()
    {
        pauseController.Push(1);
        Debug.Log("Pushed");
        UpdateTimeScale();
    }
    // Devam ettirme metodu
    public void PopPause()
    {
        if (pauseController.Count > 0)
        {
            pauseController.Pop(); // Pause stackdeki değer silinir
        } 
        Debug.Log("Popped");
        UpdateTimeScale();
    }
    // Pause stackdeki değere göre zamanın güncellenmesi
    private void UpdateTimeScale()
    {
        Time.timeScale = pauseController.Count > 0f? 0f : 1f;
        isPaused = pauseController.Count > 0f? true : false;
    }
    
}
