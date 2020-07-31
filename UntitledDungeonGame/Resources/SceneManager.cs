using Bunni.Resources.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledDungeonGame.Resources.Game;

namespace UntitledDungeonGame.Resources
{
    public static class SceneManager
    {
        public static Scene CurrentScene { get; set; }
        public static Dungeon CurrentDungeon { get; set; }
        public static Scene CurrentUIScene { get; set; }

        public static void ChangeScene(Scene s)
        {
            CurrentScene = s;
            s.Load();
        }

        public static void ChangeUiScene(Scene s)
        {
            CurrentUIScene = s;
            s.Load();
        }
    }
}
