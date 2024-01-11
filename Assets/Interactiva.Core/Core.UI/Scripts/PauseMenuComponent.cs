
using UnityEngine;
using Doozy.Engine.UI;
using UnityEngine.Events;
using Interactiva.Core.Navigation;
using Interactiva.Core.Audio;
using System.Linq;

namespace Interactiva.Core.UI
{
    public class PauseMenuComponent : UIComponent
    {
        public static PauseMenuComponent singleton;
        [SerializeField] private UnityEvent<bool> onPause;

        private UIDrawer pauseMenu;
        private IPauseEventReceiver[] pauseEventReceivers;
        private void Awake()
        {
            singleton = this;
            pauseMenu = GetComponent<UIDrawer>();
            pauseEventReceivers = FindObjectsOfType<MonoBehaviour>().OfType<IPauseEventReceiver>().ToArray();
        }
        
        public override bool Activate()
        {
            base.Activate();
            pauseMenu.Open();
            SoundManager.singleton.SetAudioAttenuationLevel(true);
            NavigationManager.singleton.SetFreezeStateAll(true);
            NavigationManager.singleton.SetCursorCanLock(false);
            SendPauseMessages(true);
            onPause.Invoke(true);
            return true;
        }
        
        public override void Deactivate()
        {
            base.Deactivate();
            pauseMenu.Close();
            SoundManager.singleton.SetAudioAttenuationLevel(false);
            NavigationManager.singleton.SetFreezeStateAll(false);
            NavigationManager.singleton.SetCursorCanLock(true);
            SendPauseMessages(false);
            onPause.Invoke(false);
        }

        private void SendPauseMessages(bool state)
        {
            foreach (IPauseEventReceiver r in pauseEventReceivers)
            {
                r.OnPauseChange(state);
            }
        }


        #region Settings Menu Events
        public void SetMasterVolume(float v)
        {
            AudioListener.volume = v;
        }

        #endregion


    }
}