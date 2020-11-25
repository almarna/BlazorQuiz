using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using BlazorQuiz.Services;
using WampSharp.V2;

namespace BlazorQuiz.Server
{
    public class QuizServer : IQuizServer
    {
        private List<Player> players = new List<Player>();
        private IWampRealmServiceProvider services;

        private int queryIndex = 0;
        private string[] queries = {"Vad kostar en ko?", "Hur långt är ett snöre?"};
        private string[] answers = {"2000", "100"};


//        private ISubject<int> subject;

        public QuizServer(IWampRealmServiceProvider services)
        {
            this.services = services;
        }

        private void Publish<T>(string topic, T data)
        {
            var subject = services.GetSubject<T>(topic);
            subject.OnNext(data);
        }

        public void StartQuiz()
        {
            Console.WriteLine("Quiz started!");
            queryIndex = 0;
            Publish("com.publish.query", queries[queryIndex]);
        }

        public void AddPlayer(string playerName)
        {
            players.Add(new Player { Name = playerName });
            var names = players.Select(item => item.Name);
            Publish("com.publish.players", names);

            Console.WriteLine($"User [{playerName}] added");
        }

        public void Answer(string playerName, string answer)
        {
            var currentPlayer = players.SingleOrDefault(player => player.Name == playerName);

            if (currentPlayer != null)
            {
                if (currentPlayer.Answers.Count == queryIndex)
                {
                    currentPlayer.Answers.Add(answer);
                    Console.WriteLine($"Player {playerName} Answer: {answer}");

                    if (AllPlayersAnswered(queryIndex))
                    {
                        GoToNextQuestion();
                    }
                }
            }
        }

        private void GoToNextQuestion()
        {
            queryIndex++;
            if (queryIndex >= queries.Length)
            {
                EndQuiz();
            }
            else
            {
                Publish("com.publish.query", queries[queryIndex]);
            }
        }

        private void EndQuiz()
        {
            Console.WriteLine("Quiz ended");
            foreach (var player in players)
            {
                player.CorrectAnswers = 0;
                int i = 0;
                foreach (var answer in player.Answers)
                {
                    if (answer == answers[i])
                    {
                        player.CorrectAnswers++;
                    }
                    i++;
                }
            }

            var playersOrder = players.OrderByDescending(item => item.CorrectAnswers);
            int position = 1;
            foreach (var player in playersOrder)
            {
                player.Position = position;
                position++;
            }
            Publish("com.publish.result", players);

        }

        private bool AllPlayersAnswered(int currentIndex)
        {
            return players.All(player => player.Answers.Count == currentIndex+1);
        }

        public IEnumerable<string> getPlayers()
        {
            return players.Select(item => item.Name);
        }
    }
}