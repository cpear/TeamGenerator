using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosballTeamGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<Player>
            {
                new Player {Name = "Yujie Wu", Rank = 7},
                new Player {Name = "Eric Fleming", Rank = 2},
                new Player {Name = "Colin Detwiller", Rank = 2},
                new Player {Name = "Vladimir Serafimov", Rank = 2},
                new Player {Name = "Nick Becker", Rank = 8},
                new Player {Name = "Scott Wilson", Rank = 6},
                new Player {Name = "Corey Keller", Rank = 7},
                new Player {Name = "Justin Mason", Rank = 8},
                new Player {Name = "Jason Reams", Rank = 5},
                new Player {Name = "Matt Sonnhalter", Rank = 4},
                new Player {Name = "Colin Pear", Rank = 6},
                new Player {Name = "Justin Self", Rank = 9},
                new Player {Name = "Steve Hickman", Rank = 5},
                new Player {Name = "Mrs. Jenkins", Rank = 2},
                new Player {Name = "Lauren", Rank = 1},
                new Player {Name = "Rayne", Rank = 4}
            };

            var generator = new TeamGenerator(players);

            generator.GenerateTeams();
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public int Rank { get; set; }
    }

    public class Team
    {
        public Player Captain { get; set; }
        public Player TeamMate { get; set; }
    }

    public class TeamGenerator
    {
        private readonly List<Player> _players;

        public TeamGenerator(List<Player> players)
        {
            _players = players;
        }

        public void GenerateTeams()
        {
            var optimumTeamRank = Math.Ceiling(_players.Select(x => x.Rank).Average()) * 2;
            var numberOfTeams = _players.Count/2;

            List<Player> playersLeft = _players.OrderByDescending(x => x.Rank).ToList();

            //Pick Captains
            List<Player> teams = new List<Player>();

            for (int i = numberOfTeams-1; i > -1; i--)
            {
                teams.Add(playersLeft[i]);
                playersLeft.RemoveAt(i);
            }

            //Shuffle the captains
            teams.Shuffle();

            //Shuffle remaining players
            playersLeft.Shuffle();

            var officialTeams = PickTeamMembers(teams, playersLeft, optimumTeamRank);

            foreach (var team in officialTeams)
            {
                Console.WriteLine(team.Captain.Name + " " + team.Captain.Rank);
                Console.WriteLine(team.TeamMate.Name + " " + team.TeamMate.Rank);
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }

        public List<Team> PickTeamMembers(List<Player> captains, List<Player> players, double optimumTeamRank)
        {
            var teams = new List<Team>();

            foreach (Player elCapitan in captains)
            {
                var team = new Team
                {
                    Captain = elCapitan
                };

                for (int j = 0; j < players.Count; j++)
                {
                    if (elCapitan.Rank + players[j].Rank <= optimumTeamRank || (j+1 == players.Count))
                    {
                        team.TeamMate = players[j];
                        teams.Add(team);

                        players.RemoveAt(j);
                        break;
                    }
                }
            }

            return teams;
        }
    }


    public static class Randomizer
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
