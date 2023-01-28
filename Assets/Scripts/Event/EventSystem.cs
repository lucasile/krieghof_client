using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Event {

    public abstract class EventSystem<T> where T : EventSystem<T> {

        public string description;

        public delegate void Listener(T eventInformation);
        private static event Listener listeners;

        public static void RegisterListener(Listener listener) {
            listeners += listener;
        }

        public static void UnregisterListener(Listener listener) {
            listeners -= listener;
        }

        public void FireEvent() {
            listeners?.Invoke(this as T);
        }

    }

}
