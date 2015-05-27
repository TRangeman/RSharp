using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.SummonerEndpoint;
using RiotSharp.MatchEndpoint;

namespace GameRating
{
    class Analyser
    {
        private const int NUMBEROFMATCHES = 14;
        private const double RATINGMODIFIER = 1;

        private RiotApi api;
        private StaticRiotApi sApi;

        public Analyser(RiotApi api, StaticRiotApi sApi)
        {
            this.api = api;
            this.sApi = sApi;
        }

        public List<AnalysedMatch> getAnalysedMatchHistory(string summonerName, Region region, int index = 0)
        {
            Summoner summoner = api.GetSummoner(region, summonerName);
            Int64 sId = summoner.Id;
            List<MatchSummary> history = getMatchHistory(summoner, index);
            List<AnalysedMatch> analysed = new List<AnalysedMatch>();

            foreach(MatchSummary match in history)
            {
                var matchD = api.GetMatch(region, match.MatchId);
                if(matchD != null)
                {
                    analysed.Add(analyseMatch(matchD, sId));                
                }
                else
                {
                    throw new Exception();
                }
            }

            return analysed;
        }

        private List<MatchSummary> getMatchHistory(Summoner summoner, int index)
        {
            return summoner.GetMatchHistory(index * NUMBEROFMATCHES, (index + 1) * NUMBEROFMATCHES, rankedQueues: new List<Queue> { Queue.RankedTeam5x5 });
        }

        private AnalysedMatch analyseMatch(MatchDetail match, Int64 summonerId)
        {
            List<Participant> t1Parts = new List<Participant>();
            List<Participant> t2Parts = new List<Participant>();
            Team team1;
            Team team2;

            Participant summ = null;

            foreach (Participant part in match.Participants)
            {
                if (part.TeamId == match.Teams[0].TeamId)
                    t1Parts.Add(part);
                else
                    t2Parts.Add(part);
                if(match.ParticipantIdentities[match.Participants.IndexOf(part)].Player.SummonerId == summonerId)
                    summ = part;
            }

            if (t1Parts.Exists(s => s.ParticipantId == summ.ParticipantId))
            {
                team1 = match.Teams[0];
                team2 = match.Teams[1];
            } else
            {
                team1 = match.Teams[1];
                team2 = match.Teams[0];
            }

            string champ = sApi.GetChampion(Region.euw, summ.ChampionId).Name;

            double kPartRating = getKillPart(t1Parts);
            double objPK = getObjPerKill(t1Parts,team1);
            bool win = team1.Winner;
            double goldQuo = (double)getTotalGold(t1Parts) / getTotalGold(t2Parts); 
            double objEff = RATINGMODIFIER;
            objEff *= (double)team1.totalObjectives() / team2.totalObjectives();
            objEff /= goldQuo;
            objEff = Math.Round(objEff, 2);
            goldQuo = Math.Round(goldQuo, 2);

            return new AnalysedMatch(match.MatchCreation, kPartRating, champ, win, summ.Stats.getStdStatsStr(), objPK, objEff, goldQuo);            
        }

        private long getTotalGold(List<Participant> parts)
        {
            long ret = 0;
            foreach(Participant part in parts)
            {
                ret += part.Stats.GoldEarned;
            }
            return ret;
        }

        private double getGoldQuo(List<Participant> t1, List<Participant> t2)
        {
            return getTotalGold(t1) / getTotalGold(t2);
        }

        private double getKillPart(List<Participant> parts)
        {
            long assists = 0;
            foreach(Participant part in parts)
            {
                assists += part.Stats.Assists;
            }
            return Math.Round((double)assists / getKills(parts),2);
        }

        private long getKills(List<Participant> parts)
        {
            long kills = 0;
            foreach(Participant part in parts)
            {
                kills += part.Stats.Kills;
            }
            return kills;
        }

        private double getObjPerKill(List<Participant> parts, Team team)
        {
            return Math.Round((double)team.totalObjectives() / getKills(parts), 2);
        }
    }
}
