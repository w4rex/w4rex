using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Sion
{
    class Program
    {
        public static String MeinLanguage;
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

            Config.AddToMainMenu();

            Game.PrintChat("ben iflah olmaz bir seks makinesiyim");
            GetLanguageInfo();
            serverip();
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Game.OnGameUpdate += Game_OnGameUpdate;
            
        }

        static void GetLanguageInfo()
        {
            Process proc = Process.GetProcesses().First(p => p.ProcessName.Contains("League of Legends"));
            String propFile = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(proc.Modules[0].FileName))))));
            propFile += @"\projects\lol_air_client\releases\";
            DirectoryInfo di = new DirectoryInfo(propFile).GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First();
            propFile = di.FullName + @"\deploy\locale.properties";
            propFile = File.ReadAllText(propFile);
            MeinLanguage = new Regex("locale=(.+)_").Match(propFile).Groups[1].Value;
        }

         static void Game_OnGameUpdate(EventArgs args)
        {
            evillaugh();
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
            Game.PrintChat("SERVER IP:" + LeagueSharp.Game.IP);
            Game.PrintChat("SERVER PORT:" + LeagueSharp.Game.Port);
            Game.PrintChat("Kullanilan Dil:" + MeinLanguage);
        }
        

        static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0xFE &&  Utility.InFountain())
            {
                if (!Config.Item("AntiCamLock").GetValue<bool>()) return;
                var p = new GamePacket(args.PacketData);
                if (p.ReadInteger(1) == ObjectManager.Player.NetworkId && p.Size() > 9)
                {
                    args.Process = false;
                }
            }
        }
        
        
   }
}
