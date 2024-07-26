using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AwaitableCoroutines
{
    public static class CoroutineAwaiterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoroutineAwaiter GetAwaiter(this IEnumerator coroutine)
        {
            return CoroutineAwaiterFactory.Get(coroutine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoroutineAwaiter GetAwaiter(this YieldInstruction instruction)
        {
            return CoroutineAwaiterFactory.Get(instruction);
        }
    };
}
