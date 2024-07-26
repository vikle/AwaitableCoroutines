using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AwaitableCoroutines
{
    public static class CoroutineAwaiterFactory
    {
        static readonly Stack<CoroutineAwaiter> sr_pool = new Stack<CoroutineAwaiter>(16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoroutineAwaiter Get(IEnumerator coroutine)
        {
            var instance = GetOrCreate();
            instance.Init(coroutine);
            return instance;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoroutineAwaiter Get(YieldInstruction instruction)
        {
            var instance = GetOrCreate();
            instance.Init(instruction);
            return instance;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CoroutineAwaiter GetOrCreate()
        {
            return (sr_pool.Count > 0) 
                ? sr_pool.Pop() 
                : new CoroutineAwaiter();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Release(CoroutineAwaiter instance)
        {
            sr_pool.Push(instance);
        }
    };
}
