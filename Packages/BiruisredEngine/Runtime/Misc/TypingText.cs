using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BiruisredEngine
{
    public class TypingText
    {
        public TypingText(float speed = 0.02f, Action<string> onSpecialTagFound = null)
        {
            Speed = speed;
            _waitFor = new WaitForTimer(Speed);
            _onSpecialTagFound = onSpecialTagFound;
        }

        public float Speed { get; set; }
        public bool IsTyping { get; private set; }
        private readonly Queue<char> _chars = new();
        private int _charLength;
        private readonly WaitForTimer _waitFor;
        private string _tempText;
        private string _fullText;
        private Action<string> _outputHandle;
        private Action<string> _onCompleteHandle;
        private Action<string> _onCompleteHandlePass;
        private Action<string> _onSpecialTagFound;
        public const string SpecialTagPattern = @"\#.*?\#";

        public IEnumerator Play(string text, Action<string> onOutput, Action<string> onComplete = null)
        {
            IsTyping = true;
            _outputHandle = onOutput;
            _onCompleteHandle = _onCompleteHandlePass = onComplete;
            _fullText = text;
            _tempText = "";
            _charLength = text.Length;
            _chars.Clear();
            bool isSpecialTag = false;
            for (var i = 0; i < _charLength; i++)
            {
                _chars.Enqueue(text[i]);
            }

            var specialTag = "";
            for (var i = 0; i < _charLength; i++)
            {
                if (!_chars.TryDequeue(out var deq)) continue;
                if (deq == '#')
                {
                    if (isSpecialTag)
                    {
                        isSpecialTag = false;
                        _onSpecialTagFound?.Invoke(specialTag);
                        specialTag = "";
                        continue;
                    }

                    isSpecialTag = true;
                    continue;
                }

                if (isSpecialTag)
                {
                    specialTag += deq;
                    continue;
                }

                _tempText += deq;
                _outputHandle?.Invoke(_tempText);
                _waitFor.Clear(Speed);
                yield return _waitFor;
            }

            IsTyping = false;
            _onCompleteHandle?.Invoke(_tempText);
        }

        public string Pass(bool onCompleteCallback = false)
        {
            IsTyping = false;
            _outputHandle = null;
            _onCompleteHandle = null;
            _waitFor.Disable();

            var matchSpecialTagPattern = Regex.Matches(_fullText, SpecialTagPattern);
            for (var i = 0; i < matchSpecialTagPattern.Count; i++)
            {
                _onSpecialTagFound?.Invoke(matchSpecialTagPattern[i].Value.Replace("#", ""));
                _fullText = _fullText.Replace(matchSpecialTagPattern[i].Value, "");
            }

            if (onCompleteCallback)
            {
                _onCompleteHandlePass.Invoke(_fullText);
            }

            return _fullText;
        }
    }
}