using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AwaitableCoroutines
{
    public sealed class AwaitableCoroutineRunner : MonoBehaviour
    {
        static AwaitableCoroutineRunner s_instance;
        static bool s_initialized;

        void OnDestroy()
        {
            s_instance = null;
            s_initialized = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StartExternalCoroutine(IEnumerator routine)
        {
            if (s_initialized == false)
            {
                Initialize();
            }

            s_instance.StartCoroutine(routine);
        }

        private static void Initialize()
        {
            var g_obj = new GameObject(nameof(AwaitableCoroutineRunner))
            {
                isStatic = true
            };

            s_instance = g_obj.AddComponent<AwaitableCoroutineRunner>();
            s_initialized = true;
            
            DontDestroyOnLoad(g_obj);
        }
    };
}
