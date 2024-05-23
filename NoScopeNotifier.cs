using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;

namespace NoScopeNotifier;

public class NoScopeNotifier : BasePlugin
{
    public override string ModuleName => "No Scope Notifier [Argentum Module]";
    public override string ModuleDescription => "NoScope Notifier - Extracted from Argentum framework - https://steamcommunity.com/id/kenoxyd";
    public override string ModuleAuthor => "kenoxyd";
    public override string ModuleVersion => "1.1.0";

    public override void Load(bool hotReload)
    { 
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
    }

    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var attacker = @event.Attacker;
        var victim = @event.Userid;

        // Early returns for invalid or irrelevant events
        if (victim == null || attacker == null || !@event.Noscope)
        {
            return HookResult.Continue;
        }

        if (!@event.Noscope)
            return HookResult.Continue;

        float distance = @event.Distance;
        string formattedDistance = distance.ToString("0.0");

        var noScopeMessages = new List<string>
        {
            $" \x0E •\x01 Insane Quickshot! \x04{attacker.PlayerName}\x01 obliterated\x04 {victim.PlayerName}\x01 from {formattedDistance}\x04 meters!",
            $" \x0E •\x01 Incredible Skill! \x04{attacker.PlayerName}\x01 vaporized\x04 {victim.PlayerName}\x01 from {formattedDistance}\x04 meters!",
            $" \x0E •\x01 Unbelievable Hail Mary! \x04{attacker.PlayerName}\x01 annihilated\x04 {victim.PlayerName}\x01 from {formattedDistance}\x04 meters!"
        };

        if (@event.Noscope)
        {
            // Announce no scope kill without special conditions
            if (!@event.Thrusmoke && !@event.Attackerinair && !@event.Attackerblind)
            {
                var randomIndex = new Random().Next(noScopeMessages.Count);
                Server.PrintToChatAll(noScopeMessages[randomIndex]);
            }
            else
            {
                string message = "";

                if (@event.Attackerblind)
                {
                    message = "blinded";
                    if (@event.Thrusmoke) message += " through smoke";
                    if (@event.Attackerinair) message += " while midair";
                }
                else
                {
                    if (@event.Thrusmoke) message = "through smoke";
                    if (@event.Attackerinair) message += (message.Length > 0 ? " " : "") + "while midair";
                }

                Server.PrintToChatAll($" \x0E• \x04{attacker.PlayerName}\x01 no-scopes, {message}, nailing \x04{victim.PlayerName}\x01 from {formattedDistance}\x04 meters!");
            }
        }

        return HookResult.Continue;
    }

}
