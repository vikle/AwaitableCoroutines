using System.Collections;
using System.Runtime.CompilerServices;

namespace AwaitableCoroutines
{
    public sealed class CoroutineAwaiterEnumerator : IEnumerator
    {
        readonly CoroutineAwaiter m_awaiter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoroutineAwaiterEnumerator(CoroutineAwaiter awaiter)
        {
            m_awaiter = awaiter;
        }

        public object Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
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

        public void Reset()
        {
            Current = null;
            m_awaiter.IsCompleted = false;
        }
    };
}
