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

            //Make the menu
            Config = new Menu("t3h 3xpl01t", "t3h 3xpl01t", true);
            
            Config.AddSubMenu(new Menu("3xpl01t", "3xpl01t"));
            Config.SubMenu("3xpl01t").AddItem(new MenuItem("AntiCamLock", "exploiti ac").SetValue(true));

            Config.AddToMainMenu();

            Game.PrintChat("ben iflah olmaz bir seks makinesiyim - tufan - ozan - onur");
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            //Casting R
            if (ObjectManager.Player.HasBuff("SionR"))
            {
                if (Config.Item("MoveToMouse").GetValue<bool>())
                {
                    var p = ObjectManager.Player.Position.To2D().Extend(Game.CursorPos.To2D(), 500);
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, p.To3D());
                }
                return;
            }

            if (Config.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                var qTarget = SimpleTs.GetTarget(!Q.IsCharging ? Q.ChargedMaxRange / 2 : Q.ChargedMaxRange, SimpleTs.DamageType.Physical);

                var eTarget = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);

                if (qTarget != null && Config.Item("UseQCombo").GetValue<bool>())
                {
                    if (Q.IsCharging)
                    {
                        var start = ObjectManager.Player.ServerPosition.To2D();
                        var end = start.Extend(QCastPos, Q.Range);
                        var direction = (end - start).Normalized();
                        var normal = direction.Perpendicular();

                        var points = new List<Vector2>();
                        var hitBox = qTarget.BoundingRadius;
                        points.Add(start + normal * (Q.Width + hitBox));
                        points.Add(start - normal * (Q.Width + hitBox));
                        points.Add(end + Q.ChargedMaxRange * direction - normal * (Q.Width + hitBox));
                        points.Add(end + Q.ChargedMaxRange * direction + normal * (Q.Width + hitBox));

                        for (int i = 0; i <= points.Count - 1; i++)
                        {
                            var A = points[i];
                            var B = points[i == points.Count - 1 ? 0 : i + 1];

                            if (qTarget.ServerPosition.To2D().Distance(A, B, true, true) < 50 * 50)
                            {
                                Packet.C2S.ChargedCast.Encoded(new Packet.C2S.ChargedCast.Struct((SpellSlot)((byte)Q.Slot), Game.CursorPos.X, Game.CursorPos.X, Game.CursorPos.X)).Send();
                            }
                        }
                        return;
                    }
                    
                    if(Q.IsReady())
                    {
                        Q.StartCharging(qTarget.ServerPosition);
                    }
                }

                if (qTarget != null && Config.Item("UseWCombo").GetValue<bool>())
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, ObjectManager.Player);
                }

                if (eTarget != null && Config.Item("UseECombo").GetValue<bool>())
                {
                    E.Cast(eTarget);
                }
            }
        }
    }
}
