﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.MatchEndpoint;
using RiotSharp.ChampionEndpoint;

namespace GameRating
{
    class AnalysedMatch
    {
        public DateTime date { private set; get; }
        public string champion { private set; get; }
        public string ownStats { private set; get; }

        public bool win { private set; get; }
        public double kPart { private set; get; }
        public double objPerKill { private set; get; }


        public AnalysedMatch(DateTime time, double rating, string champ, bool win, string stats, double objPK)
        {
            this.win = win;
            objPerKill = objPK;
            date = time;
            champion = champ;
            kPart = rating;
            ownStats = stats;
        }
    }
}
