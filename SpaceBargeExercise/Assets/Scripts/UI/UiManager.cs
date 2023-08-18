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
        public MainMenu mainMenu;
        public GameOrtho ortho;

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainMenu.Init();
            mainMenu.play.onClick.AddListener(() => mainMenu.SwitchToMenu(ortho));
            ShowMain();
        }

        public void ShowMain() 
        {
            mainMenu.Show();
            ortho.Hide();
        }
    }

    [System.Serializable]
    public abstract class UiMenu 
    {
        [SerializeField]protected RectTransform parentContainer;
        public virtual void Show() => parentContainer.gameObject.SetActive(true);
        public virtual void Hide() => parentContainer.gameObject.SetActive(false);
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

        public void Init() 
        {
            play.onClick.AddListener(() => GameManager.instance.LoadLevel(0));
            settings.interactable = false;// Still in develoipment.
            exit.onClick.AddListener(Application.Quit);
        }
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
