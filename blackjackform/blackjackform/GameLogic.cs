using System;
using System.Collections.Generic;
using System.Linq;

namespace blackjackform
{
    public static class GameLogic
    {
        public static List<Card> GetShuffledDeck()
        {
            var deck = new List<Card>();
            string[] suits = { "♥", "♦", "♣", "♠" }; // Порядок мастей соответствует Deck2.png
            string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            foreach (var suit in suits)
                foreach (var rank in ranks)
                    deck.Add(new Card(suit, rank));

            return deck.OrderBy(_ => new Random().Next()).ToList();
        }

        public static int CalculateScore(List<Card> cards)
        {
            int score = 0;
            int aces = 0;

            foreach (var card in cards)
            {
                if (card.Rank == "A")
                    aces++;
                else if (card.Rank == "K" || card.Rank == "Q" || card.Rank == "J")
                    score += 10;
                else
                    score += int.Parse(card.Rank);
            }

            for (int i = 0; i < aces; i++)
            {
                if (score + 11 <= 21)
                    score += 11;
                else
                    score += 1;
            }

            return score;
        }

        public static void PlayCrupeTurn(List<Card> deck, List<Card> crupe)
        {
            while (CalculateScore(crupe) < 17)
            {
                crupe.Add(deck.PopLast());
            }
        }

        public static string DetermineWinner(List<Card> hand, List<Card> crupe)
        {
            int playerScore = CalculateScore(hand);
            int crupeScore = CalculateScore(crupe);

            if (playerScore > 21)
                return "Вы проиграли!";
            else if (crupeScore > 21 || playerScore > crupeScore)
                return "Вы выиграли!";
            else if (playerScore < crupeScore)
                return "Крупье выиграл!";
            else
                return "Ничья!";
        }
    }

    public class Card
    {
        public string Suit { get; }
        public string Rank { get; }

        public Card(string suit, string rank)
        {
            Suit = suit ?? throw new ArgumentNullException(nameof(suit));
            Rank = rank ?? throw new ArgumentNullException(nameof(rank));
        }

        public override string ToString() => $"{Rank}{Suit}";
    }

    public static class ListExtensions
    {
        public static T PopLast<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("Список пуст");

            int lastIndex = list.Count - 1;
            T item = list[lastIndex];
            list.RemoveAt(lastIndex);
            return item;
        }
    }
}