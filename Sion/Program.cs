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

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {

            //Make the menu
            Config = new Menu("t3h 3xpl01t", "t3h 3xpl01t", true);
            
            Config.AddSubMenu(new Menu("3xpl01t", "3xpl01t"));
            Config.SubMenu("3xpl01t").AddItem(new MenuItem("AntiCamLock", "yarramin basi").SetValue(true));
            Config.SubMenu("3xpl01t").AddItem(new MenuItem("spam", "spamin basi").SetValue(true));
            Config.SubMenu("3xpl01t").AddItem(new MenuItem("spam2", "i i follow i follow you").SetValue(true));
            Config.SubMenu("3xpl01t").AddItem(new MenuItem("serverip", "server ip al").SetValue(true));

            Config.AddToMainMenu();

            Game.PrintChat("ben iflah olmaz bir seks makinesiyim");
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Game.OnGameUpdate += Game_OnGameUpdate;
            
        }

         static void Game_OnGameUpdate(EventArgs args)
        {
            evillaugh();
            serverip();
            garipspam();
        }

        static void evillaugh()
        {
            if (!Config.Item("spam").GetValue<bool>()) return;
            Packet.C2S.Emote.Encoded(new Packet.C2S.Emote.Struct(2)).Send();
            if (!Config.Item("spam2").GetValue<bool>()) return;
            Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(Game.CursorPos.X, Game.CursorPos.Y)).Send();
        }
        
        static void serverip()
        {
            if (!Config.Item("serverip").GetValue<bool>()) return;
            Game.PrintChat("SERVER IP:" + LeagueSharp.Game.IP);
            Game.PrintChat("SERVER PORT:" + LeagueSharp.Game.Port);
            return;
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
