using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AwaitableCoroutines
{
    public sealed class CoroutineAwaiter : ICriticalNotifyCompletion
    {
        public IEnumerator Coroutine { [MethodImpl(MethodImplOptions.AggressiveInlining)]get; }
        public YieldInstruction Instruction { [MethodImpl(MethodImplOptions.AggressiveInlining)]get; }

        readonly CoroutineAwaiterEnumerator m_controller;
        
        Action m_continuation;
        bool m_isCompleted;

        public bool IsCompleted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get => m_isCompleted;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set
            {
                m_isCompleted = value;
                if (value == false) return;
                m_continuation?.Invoke();
                m_continuation = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CoroutineAwaiter()
        {
            m_controller = new CoroutineAwaiterEnumerator(this);
            AwaitableCoroutineRunner.StartExternalCoroutine(m_controller);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoroutineAwaiter(IEnumerator enumerator) : this()
        {
            Coroutine = enumerator;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoroutineAwaiter(YieldInstruction instruction) : this()
        {
            Instruction = instruction;
        }
        
        public void OnCompleted(Action continuation)
        {
            m_continuation = continuation;
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            m_continuation = continuation;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetResult()
        {
            return m_controller.Current;
        }
    };
    
    public static class CoroutineAwaiterExtensions
    {
        public static CoroutineAwaiter GetAwaiter(this IEnumerator coroutine)
        {
            return new CoroutineAwaiter(coroutine);
        }

        public static CoroutineAwaiter GetAwaiter(this YieldInstruction instruction)
        {
            return new CoroutineAwaiter(instruction);
        }
    };
}
