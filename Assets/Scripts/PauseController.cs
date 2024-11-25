using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool paused;

    /// <summary>
    /// Меняет состояние игры
    /// </summary>
    public void ChangePause()
    {
        if (paused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        paused = !paused;
    }
}
