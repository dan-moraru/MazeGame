using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MazeGame
{
    public class InputManager
    {
        private static InputManager _instance = null;
        private KeyboardState _previousState;

        private Dictionary<Keys, List<Action>> _keyHandlers = new Dictionary<Keys, List<Action>>();

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InputManager();
                }
                return _instance;
            }
        }

        public void AddKeyHandler(Keys key, Action action)
        {
            if (!_keyHandlers.ContainsKey(key))
            {
                _keyHandlers[key] = new List<Action>();
            }
            _keyHandlers[key].Add(action);
        }

        public void Update()
        {

            Dictionary<Keys, List<Action>> _keyHandlersClone = CloneKeyHandlers(_keyHandlers);

            KeyboardState keyboardState = Keyboard.GetState();
            foreach (var key in _keyHandlersClone.Keys)
            {
                if (keyboardState.IsKeyDown(key) & !_previousState.IsKeyDown(key))
                {
                    foreach (var action in _keyHandlers[key])
                    {
                        action.Invoke();
                    }
                }
            }
            _previousState = keyboardState;
        }

        //Deep copy of KeyHandlers to add extra key handlers while looping through update in MazeGame
        public Dictionary<Keys, List<Action>> CloneKeyHandlers<Keys, Action>(Dictionary<Keys, List<Action>> original) where Action : ICloneable
        {
            Dictionary<Keys, List<Action>> deepCopy = new Dictionary<Keys, List<Action>>(original.Count, original.Comparer);
            foreach (KeyValuePair<Keys, List<Action>> entry in original)
            {
                deepCopy.Add(entry.Key, new List<Action>(entry.Value));
            }
            return deepCopy;
        }
    }
}
