using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Sion
{
    class Program
    {
        private static Menu Config;

        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q;
        public static Spell E;

        public static Vector2 QCastPos = new Vector2();
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {

            //Spells
            Q = new Spell(SpellSlot.Q, 1050);
            Q.SetSkillshot(0.6f, 100f, float.MaxValue, false, SkillshotType.SkillshotLine);
            Q.SetCharged("SionQ", "SionQ", 500, 720, 0.5f);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.25f, 80f, 1800, false, SkillshotType.SkillshotLine);

            //Make the menu
            Config = new Menu("t3h 3xpl01t", "t3h 3xpl01t", true);
            
            Config.AddSubMenu(new Menu("3xpl01t", "3xpl01t"));
            Config.SubMenu("3xpl01t").AddItem(new MenuItem("AntiCamLock", "exploiti aç").SetValue(true));

            Config.AddToMainMenu();

            Game.PrintChat("t3h 3xplo1t l04d3d - w4rex - yani ozan - yani uğur ozan - yani puro - yani reyiz!");
            Game.OnGameUpdate += Game_OnGameUpdate;
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0xFE && Config.Item("AntiCamLock").GetValue<bool>())
            {
                var p = new GamePacket(args.PacketData);
                if (p.ReadInteger(1) == ObjectManager.Player.NetworkId && p.Size() > 9)
                {
                    args.Process = false;
                }
            }
        }
            }
        }
