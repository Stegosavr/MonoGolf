using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Snakedy
{
    public class ScoreManager
    {
        private static string _scoresFileName = "scores.xml";
        private static string _latestScoreFileName = "latest score.xml";
        private static List<Score> _defaultScores = new List<Score>()
        {
            new Score() { PlayerName = "Bob", Value = 10},
            new Score() { PlayerName = "Mike", Value = 30},
            new Score() { PlayerName = "Jacob", Value = 100}
        };

        public Score LatestScore { get; private set; }
        public List<Score> Scores { get; private set; }
        public List<Score> Highscores { get; private set; }

        public ScoreManager() : 
            this(_defaultScores)
        {

        }

        public ScoreManager(List<Score> scores)
        {
            this.Scores = scores;
            UpdateHighscores();
        }

        public void EditLatestScore(string playerName)
        {
            LatestScore.PlayerName = playerName;
        }

        public void Add(Score score)
        {
            LatestScore = score;
            Scores.Add(score);

            Scores = Scores
                .OrderByDescending(score=>score.Value)
                .DistinctBy(score=>score.PlayerName)//+score.Value)
                .ToList();

            UpdateHighscores();
        }

        public static ScoreManager Load()
        {
            if (!File.Exists(_scoresFileName))
                return new ScoreManager();

            using (var reader = new StreamReader(new FileStream(_latestScoreFileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(Score));
                Globals.PlayerName = ((Score)serializer.Deserialize(reader)).PlayerName;
            }

            using (var reader = new StreamReader(new FileStream(_scoresFileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));
                var scores = (List<Score>)serializer.Deserialize(reader);

                return new ScoreManager(scores);
            }
        }

        public static void Save(ScoreManager scoreManager)
        {
            using (var writer = new StreamWriter(new FileStream(_scoresFileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));
                serializer.Serialize(writer, scoreManager.Scores);
            }
            using (var writer = new StreamWriter(new FileStream(_latestScoreFileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(Score));
                serializer.Serialize(writer, scoreManager.LatestScore);
            }

            Globals.Scores = scoreManager.Scores;
        }

        private void UpdateHighscores()
        {
            Highscores = Scores.Take(5).ToList();
        }
    }
}
