using System;
using UnityEngine;

namespace ProBase
{
    public class LoadingProgress
    {
        private float _value;

        public event Action Changed;

        public float Value => _value;

        public void Report(float value)
        {
            float clamped = Mathf.Clamp01(value);

            if (Mathf.Approximately(clamped, _value)) return;

            _value = clamped;
            Changed?.Invoke();
        }

        public void Reset()
        {
            _value = 0f;
            Changed?.Invoke();
        }
    }
}
