using System;
using UnityEngine;

public class ActionsManager
{
    private static ActionsManager instance;

    #region Properties

    public static ActionsManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new ActionsManager();
            }
            return instance; 
        }
        private set { instance = value; }
    }

    #endregion

    public Action onPlayerDeath;
    public Action onPlayerEnterCar;
    public Action onPlayerExitCar;

    public Action<Vector2> onPlayerMoveInput;
    public Action<float> onPlayerSprintInput;
    public Action<float> onPlayerJumpInput;
    public Action<float> onPlayerInteractInput;

    public Action onPlayerRagdollActivate;
    public Action onPlayerRagdollDeactivate;

    public Action<float, Vector3> onPlayerChangeSpeed;
}
