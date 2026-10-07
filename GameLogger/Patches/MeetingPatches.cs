using System.Collections.Generic;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using AmongUs.GameOptions;

namespace GameLogger
{
    [HarmonyPatch]

    public class MeetingLogs
    {
        public static HashSet<byte> GhostsLogged = new();
        public static List<byte> VoteOrder = new();

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CoIntro))]
        [HarmonyPostfix]

        public static void Start(ref NetworkedPlayerInfo reporter, ref NetworkedPlayerInfo reportedBody, ref Il2CppReferenceArray<NetworkedPlayerInfo> deadBodies)
        {
            VoteOrder.Clear();

            string action = reportedBody == null ? "This is a emergency meeting" : $"{Utils.FullName(reportedBody)}'s body was found";
            string bodytext = "Players died this round: ";

            if (deadBodies.Length == 0)
            {
                bodytext = "No one died this round";
            }
            else
            {
                foreach (var body in deadBodies)
                {
                    bodytext += $"{Utils.FullName(body)}, ";
                }
                bodytext = bodytext.Remove(bodytext.LastIndexOf(","));
            }

            Utils.Write($"Meeting started by {Utils.FullName(reporter)}", action, bodytext);

        }

        // Fires once per player as their vote registers - records the real cast order
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        [HarmonyPostfix]

        public static void RecordVote(ref byte srcPlayerId)
        {
            if (!VoteOrder.Contains(srcPlayerId))
            {
                VoteOrder.Add(srcPlayerId);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        [HarmonyPostfix]

        public static void CheckVotes(ref Il2CppStructArray<MeetingHud.VoterState> states)
        {
            if (!GameLogger.LogVotes.Value) return;

            string text = "Vote results:\n";

            // Reorder states to match the order votes were actually cast in
            var consumed = new HashSet<byte>();
            var ordered = new List<MeetingHud.VoterState>();

            foreach (var voterId in VoteOrder)
            {
                foreach (var vote in states)
                {
                    if (vote.VoterId == voterId && consumed.Add(vote.VoterId))
                    {
                        ordered.Add(vote);
                        break;
                    }
                }
            }

            // Anyone who never cast a vote (disconnected, ran out the clock, etc.) goes at the end
            foreach (var vote in states)
            {
                if (consumed.Add(vote.VoterId))
                {
                    ordered.Add(vote);
                }
            }

            foreach (var vote in ordered)
            {
                if (!vote.AmDead)
                {
                    var voter = Utils.GetPlayer(vote.VoterId);
                    if (voter != null)
                    {
                        if (vote.SkippedVote)
                        {
                            text += $"{Utils.FullName(voter)} skipped\n";
                        }
                        else
                        {
                            if (vote.VotedForId == 254)
                            {
                                text += $"{Utils.FullName(voter)} did not vote\n";
                            }
                            else
                            {
                                if (vote.VotedForId != byte.MaxValue)
                                {
                                    var votedFor = Utils.GetPlayer(vote.VotedForId);
                                    text += $"{Utils.FullName(voter)} voted for {Utils.FullName(votedFor)}\n";
                                }
                            }
                        }
                    }
                }
            }
            Utils.Write(text);
        }


        [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
        [HarmonyPostfix]

        public static void End(ExileController __instance)
        {
            Utils.Write(__instance.completeString);
        }
    }
}