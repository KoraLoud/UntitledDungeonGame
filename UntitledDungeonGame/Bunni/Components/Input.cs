using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunni.Resources.Modules;


//TODO:
//Add gamepad input

namespace Bunni.Resources.Components
{
    public class Input : Component
    {
        public Vector2 InputVector { get; set; }

        private List<KeyListener> Listeners = new List<KeyListener>();

        /// <summary>
        /// Sets default movement keys to WASD
        /// </summary>
        /// <returns>keyboard input component</returns>
        public void SetDefaultKeyboardKeys()
        {
            _SetDefaultKeyboardKeys(Keys.W, Keys.A, Keys.S, Keys.D);
        }

        /// <summary>
        /// Sets default movement keys to custom keys.
        /// </summary>
        /// <param name="UpK">Key to set input vector Y position to -1</param>
        /// <param name="LeftK">Key to set input vector X position to -1</param>
        /// <param name="DownK">Key to set input vector Y position to 1</param>
        /// <param name="RightK">Key to set input vector X position to 1</param>
        /// <returns></returns>
        public void SetDefaultKeyboardKeys(Keys UpK, Keys LeftK, Keys DownK, Keys RightK)
        {
            _SetDefaultKeyboardKeys(UpK, LeftK, DownK, RightK);
        }

        private void _SetDefaultKeyboardKeys(Keys UpK, Keys LeftK, Keys DownK, Keys RightK)
        {
            BindKey(UpK, (pressed, held) =>
            {
                Vector2 input = InputVector;
                if (pressed || held)
                {
                    input.Y = -1;
                }
                else if (input.Y == -1)
                {
                    input.Y = 0;
                }
                InputVector = input;
            });

            BindKey(RightK, (pressed, held) =>
            {
                Vector2 input = InputVector;
                if (pressed || held)
                {
                    input.X = 1;
                }
                else if (input.X == 1)
                {
                    input.X = 0;
                }
                InputVector = input;
            });

            BindKey(DownK, (pressed, held) =>
            {
                Vector2 input = InputVector;
                if (pressed || held)
                {
                    input.Y = 1;
                }
                else if (input.Y == 1)
                {
                    input.Y = 0;
                }
                InputVector = input;
            });

            BindKey(LeftK, (pressed, held) =>
            {
                Vector2 input = InputVector;
                if (pressed || held)
                {
                    input.X = -1;
                }else if(input.X==-1)
                {
                    input.X = 0;
                }
                InputVector = input;
            });
        }

        public override void Update(GameTime gameTime, Scene scene)
        {

            KeyboardState keyboard = Keyboard.GetState(); //get keyboard state
            Listeners.ForEach((listener) => //loop through every listener
            {
                if (keyboard.IsKeyDown(listener.KeyToListen)) 
                {
                    if (!listener.Down) //if the key is down, and the listener is not set to down, set to down.
                    {
                        listener.Down = true;
                    }else if (!listener.DownLastFrame) //if listener key is set to down, but down last frame is not, then set it to true
                    {
                        listener.DownLastFrame = true;
                    }
                }else //key not being pressed
                {
                    if (listener.Down) //if the key in the listener is set to down, fix that
                    {
                        listener.Down = false;
                    }else if (listener.DownLastFrame) //if key is set to down, but DownLastFrame is true, set to false
                    {
                        listener.DownLastFrame = false;
                    }
                }

                if(listener.Down && !listener.DownLastFrame) //if the key is being pressed, but was not pressed last frame, call function
                {
                    listener.KeyPressedCallback(true, false);
                }else if(listener.Down && listener.DownLastFrame)
                {
                    listener.KeyPressedCallback(false, true);
                }else
                {
                    listener.KeyPressedCallback(false, false);
                }
            });

        }

        /// <summary>
        /// Checks if a key is being held.
        /// </summary>
        /// <returns></returns>
        public bool KeyIsHeld(Keys keyToCheck)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(keyToCheck);
        }

        /// <summary>
        /// Checks to see if key bind exists for key
        /// </summary>
        /// <param name="keyToFind">The key to check key bind for</param>
        /// <returns>true or false</returns>
        public bool KeyBindExists(Keys keyToFind)
        {
            KeyListener keyFound = GetKeyListenerObject(keyToFind);
            if(keyFound != null)
            {
                return true;
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// Connects a listener to the user's input device.
        /// The callback will fire when the Key is pressed by the user.
        /// </summary>
        /// <param name="keyToListen">The key to listen for press from user</param>
        /// <param name="callback">The function that will call when the key is pressed</param>
        public void BindKey(Keys keyToListen, Action<bool, bool> callback)
        {
            //make sure key isnt already connected
            KeyListener searchForKey = GetKeyListenerObject(keyToListen);
            if (searchForKey == null)
            {
                KeyListener newBind = new KeyListener(keyToListen, callback);
                Listeners.Add(newBind);
            }else
            {
                Console.WriteLine("This key is already bound");
            }
        }

        /// <summary>
        /// Edits the bind of a key, changes the button pressed and the callback
        /// </summary>
        /// <param name="keyToEdit">Key you are trying to edit</param>
        /// <param name="newKey">Sets the bind to a new key</param>
        /// <param name="callback">Changes what happens when you press the key</param>
        public void EditBind(Keys keyToEdit, Keys newKey, Action<bool, bool> callback)
        {
            KeyListener listenerToEdit = GetKeyListenerObject(keyToEdit);
            if (listenerToEdit != null)
            {
                listenerToEdit.KeyToListen = newKey;
                listenerToEdit.KeyPressedCallback = callback;
            }else
            {
                Console.WriteLine("This key bind does not exist");
            }
        }
        /// <summary>
        /// Changes the callback to a keybind
        /// </summary>
        /// <param name="keyToEdit">Key you are editing the bind of</param>
        /// <param name="callback">What will happen when the key is pressed</param>
        public void EditBind(Keys keyToEdit, Action<bool, bool> callback)
        {
            KeyListener listenerToEdit = GetKeyListenerObject(keyToEdit);
            if (listenerToEdit != null)
            {
                listenerToEdit.KeyPressedCallback = callback;
            }
            else
            {
                Console.WriteLine("This key bind does not exist");
            }
        }

        /// <summary>
        /// Change the keybind of a key to a new key
        /// </summary>
        /// <param name="keyToEdit">Key of bind that is being changed</param>
        /// <param name="newKey">New key to bind to</param>
        public void EditBind(Keys keyToEdit, Keys newKey)
        {
            KeyListener listenerToEdit = GetKeyListenerObject(keyToEdit);
            if (listenerToEdit != null)
            {
                listenerToEdit.KeyToListen = newKey;
            }
            else
            {
                Console.WriteLine("This key bind does not exist");
            }
        }

        /// <summary>
        /// Gets KeyListener object from Listeners List
        /// </summary>
        /// <param name="keyToGet">The key of the bind being retrieved</param>
        /// <returns>The KeyListener object of the key</returns>
        private KeyListener GetKeyListenerObject(Keys keyToGet)
        {
            return Listeners.Find(l => l.KeyToListen == keyToGet);
        }

        /// <summary>
        /// A private KeyListener object used to keep up with the properties of the key bind
        /// </summary>
        private class KeyListener
        {
            public Keys KeyToListen { get; set; }
            public bool Down { get; set; }
            public bool DownLastFrame { get; set; }
            public Action<bool, bool> KeyPressedCallback;

            public KeyListener(Keys _keyToListen, Action<bool,bool> _callback)
            {
                KeyToListen = _keyToListen;
                KeyPressedCallback = _callback;
            }
        }


        /// <summary>
        /// Returns where in the game world the mouse is pointing
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetGlobalMousePosition()
        {
            MouseState mouse = Mouse.GetState();
            Vector2 mouseCoors = new Vector2(mouse.X, mouse.Y);
            Vector2 WorldPos = Vector2.Transform(mouseCoors, Matrix.Invert(Camera.Transform));
            return WorldPos;
        }

    }
}
