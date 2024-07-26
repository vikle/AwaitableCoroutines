using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AwaitableCoroutines
{
    public static class CoroutineAwaiterEnumeratorFactory
    {
        static readonly Stack<CoroutineAwaiterEnumerator> sr_pool = new Stack<CoroutineAwaiterEnumerator>(16);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoroutineAwaiterEnumerator Get(CoroutineAwaiter awaiter)
        {
            var instance = (sr_pool.Count > 0) 
                ? sr_pool.Pop() 
                : new CoroutineAwaiterEnumerator();
            
            instance.Init(awaiter);
            
            return instance;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Release(CoroutineAwaiterEnumerator instance)
        {
            sr_pool.Push(instance);
        }
    };
}
