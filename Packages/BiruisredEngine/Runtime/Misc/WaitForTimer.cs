using UnityEngine;

namespace BiruisredEngine
{
    public class WaitForTimer : CustomYieldInstruction
    {
        public WaitForTimer(float second)
        {
            _time = second;
            _enable = true;
        }

        private float _time;
        private float _currentTime;
        private bool _enable;

        public override bool keepWaiting
        {
            get
            {
                if (!_enable) return false;
                _currentTime += Time.deltaTime;
                return _currentTime < _time;
            }
        }

        /// <summary>
        /// Clear timer and enable state
        /// </summary>
        public void Clear()
        {
            _currentTime = 0;
            _enable = true;
        }

        /// <summary>
        /// Clear timer and enable state
        /// </summary>
        /// <param name="newTime">new set time in second</param>
        public void Clear(float newTime)
        {
            _time = newTime;
            _currentTime = 0;
            _enable = true;
        }

        public void Enable()
        {
            _enable = true;
        }

        public void Disable()
        {
            _enable = false;
        }
    }
}