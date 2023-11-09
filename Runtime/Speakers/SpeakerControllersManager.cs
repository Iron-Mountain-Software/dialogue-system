using System;
using System.Collections.Generic;

namespace IronMountain.DialogueSystem.Speakers
{
    public static class SpeakerControllersManager
    {
        public static event Action OnSpeakerControllersChanged;
        
        public static readonly List<SpeakerController> SpeakerControllers = new ();

        public static void Register(SpeakerController speakerController)
        {
            if (!speakerController || SpeakerControllers.Contains(speakerController)) return;
            SpeakerControllers.Add(speakerController);
            OnSpeakerControllersChanged?.Invoke();
        }
        
        public static void Unregister(SpeakerController speakerController)
        {
            if (!speakerController || !SpeakerControllers.Contains(speakerController)) return;
            SpeakerControllers.Remove(speakerController);
            OnSpeakerControllersChanged?.Invoke();
        }
    }
}
