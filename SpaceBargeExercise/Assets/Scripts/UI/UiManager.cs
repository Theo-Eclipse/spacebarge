using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flier.Controls;
using Flier;

namespace UnityEngine.UI
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager instance { get; private set; }
        public GameOrtho ortho;

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame

    }

    [System.Serializable]
    public abstract class UiMenu 
    {
        [SerializeField]protected RectTransform parentContainer;
        protected virtual void Show() => parentContainer.gameObject.SetActive(false);
        protected virtual void Hide() => parentContainer.gameObject.SetActive(true);
        public virtual void SwitchToMenu(UiMenu otherMenu) 
        {
            otherMenu.Show();
            Hide();
        }
    }
    [System.Serializable]
    public class MainMenu : UiMenu 
    {
        [Header("Menu Buttons")]
        [Space] public Button play;
        public Button settings;
        public Button exit;
    }
    [System.Serializable]
    public class SettingsMenu : UiMenu
    {
        [Header("Settings Buttons")]
        [Space] public Button backToMenu;
    }
    [System.Serializable]
    public class GameOrtho : UiMenu
    {
        [Header("Ortho Buttons")]
        [Space] public Button inGameMenu;
        [SerializeField] private ToggleFire fireControl;
        [SerializeField] private UiJoystick orthoJoystick;
        public void SetControls(DefaultFlierControl controlTarget)
        {
            controlTarget.screenJoystick = orthoJoystick;
            fireControl.SelectTargetFlier(controlTarget);
        }
    }
}
