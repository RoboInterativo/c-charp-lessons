using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("Hello, World!");
Console.WriteLine("Сдает по 2 карты, себе и вам");

var deck = GetShuffledDeck();
var crupe = new List<Card>(); // Карты крупье
var hand = new List<Card>();  // Ваши карты

// Раздача начальных карт
crupe.Add(deck.PopLast());
hand.Add(deck.PopLast());
crupe.Add(deck.PopLast());
hand.Add(deck.PopLast());

// Игровой цикл
bool playerTurn = true;
while (playerTurn)
{
    // Показываем карты
    Console.Clear();
    Console.WriteLine("\nВаши карты:");
    foreach (var card in hand) Console.WriteLine(card);
    Console.WriteLine($"Очки: {CalculateScore(hand)}");

    Console.WriteLine("\nКарты крупье:");
    Console.WriteLine(crupe[0]); // Показываем только первую карту крупье
    Console.WriteLine("[Скрытая карта]");

    // Запрашиваем действие
    Console.WriteLine("\nНажмите H (hit) для взятия карты, S (stand) для остановки");
    var key = Console.ReadKey(true).Key;

    switch (key)
    {
        case ConsoleKey.H:
            hand.Add(deck.PopLast());
            int playerScore = CalculateScore(hand);
            if (playerScore > 21)
            {
                Console.Clear();
                ShowFinalResults(hand, crupe);
                Console.WriteLine("Перебор! Вы проиграли!");
                playerTurn = false;
            }
            break;

        case ConsoleKey.S:
            playerTurn = false;
            PlayCrupeTurn(deck, crupe);
            ShowFinalResults(hand, crupe);
            DetermineWinner(hand, crupe);
            break;
    }
}

// Методы игры
static void ShowFinalResults(List<Card> hand, List<Card> crupe)
{
    Console.WriteLine("\nВаши карты:");
    foreach (var card in hand) Console.WriteLine(card);
    Console.WriteLine($"Очки: {CalculateScore(hand)}");

    Console.WriteLine("\nКарты крупье:");
    foreach (var card in crupe) Console.WriteLine(card);
    Console.WriteLine($"Очки: {CalculateScore(crupe)}");
}

static void PlayCrupeTurn(List<Card> deck, List<Card> crupe)
{
    while (CalculateScore(crupe) < 17)
    {
        crupe.Add(deck.PopLast());
    }
}

static void DetermineWinner(List<Card> hand, List<Card> crupe)
{
    int playerScore = CalculateScore(hand);
    int crupeScore = CalculateScore(crupe);

    if (playerScore > 21)
        Console.WriteLine("Вы проиграли!");
    else if (crupeScore > 21 || playerScore > crupeScore)
        Console.WriteLine("Вы выиграли!");
    else if (playerScore < crupeScore)
        Console.WriteLine("Крупье выиграл!");
    else
        Console.WriteLine("Ничья!");
}

static int CalculateScore(List<Card> cards)
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

    // Обрабатываем тузы
    for (int i = 0; i < aces; i++)
    {
        if (score + 11 <= 21)
            score += 11;
        else
            score += 1;
    }

    return score;
}

static List<Card> GetShuffledDeck()
{
    var deck = new List<Card>();
    string[] suits = { "♠", "♥", "♦", "♣" };
    string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    foreach (var suit in suits)
        foreach (var rank in ranks)
            deck.Add(new Card(suit, rank));

    return deck.OrderBy(_ => new Random().Next()).ToList();
}

class Card
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
