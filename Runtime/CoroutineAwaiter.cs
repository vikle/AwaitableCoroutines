using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AwaitableCoroutines
{
    public sealed class CoroutineAwaiter : ICriticalNotifyCompletion
    {
        public IEnumerator Coroutine
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        public YieldInstruction Instruction
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        CoroutineAwaiterEnumerator m_controller;
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
                Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(IEnumerator enumerator)
        {
            Coroutine = enumerator;
            Init();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(YieldInstruction instruction)
        {
            Instruction = instruction;
            Init();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Init()
        {
            m_controller = CoroutineAwaiterEnumeratorFactory.Get(this);
            AwaitableCoroutineRunner.StartExternalCoroutine(m_controller);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Dispose()
        {
            CoroutineAwaiterFactory.Release(this);
            
            m_isCompleted = false;

            Coroutine = null;
            Instruction = null;
            m_continuation = null;

            m_controller.Dispose();
            m_controller = null;
        }
    };
}
