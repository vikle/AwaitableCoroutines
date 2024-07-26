using System.Collections;
using System.Runtime.CompilerServices;

namespace AwaitableCoroutines
{
    public sealed class CoroutineAwaiterEnumerator : IEnumerator
    {
        CoroutineAwaiter m_awaiter;

        public object Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(CoroutineAwaiter awaiter)
        {
            m_awaiter = awaiter;
        }
        
        public bool MoveNext()
        {
            var awaiter = m_awaiter;
            var coroutine = awaiter.Coroutine;

            if (coroutine != null)
            {
                bool result = coroutine.MoveNext();
                Current = coroutine.Current;
                awaiter.IsCompleted = !result;
                return result;
            }

            if (Current == null)
            {
                Current = awaiter.Instruction;
                return true;
            }

            awaiter.IsCompleted = true;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Current = null;
            m_awaiter.IsCompleted = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            CoroutineAwaiterEnumeratorFactory.Release(this);
            
            m_awaiter = null;
            Current = null;
        }
    };
}
