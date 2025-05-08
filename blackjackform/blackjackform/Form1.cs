using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace blackjackform
{
    public partial class Form1 : Form
    {
        private Image deckImage;
        private List<Card> deck;
        private List<Card> playerHand;
        private List<Card> dealerHand;
        private bool playerTurn;
        private const int SizeX = 70;  // Размер карты по ширине
        private const int SizeY = 105; // Размер карты по высоте
        private const int SizeBX = 75; // Размер блока по ширине (70 + 5)
        private const int SizeBY = 109; // Размер блока по высоте (105 + 2)
        private const int StartX = 5;  // Начальная точка X
        private const int StartY = 5;  // Начальная точка Y
        private const int OffsetY = 0; // Коррекция по высоте

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            try
            {
                deckImage = Image.FromFile("Deck2.png");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Файл Deck2.png не найден. Убедитесь, что он находится в папке {AppDomain.CurrentDomain.BaseDirectory} или настроен для копирования в выходную папку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // Инициализация игры
            deck = GameLogic.GetShuffledDeck();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();
            playerTurn = true;

            // Раздача начальных карт
            dealerHand.Add(deck.PopLast());
            playerHand.Add(deck.PopLast());
            dealerHand.Add(deck.PopLast());
            playerHand.Add(deck.PopLast());

            // Настраиваем форму
            this.Text = "Блэкджек";
            this.ClientSize = new Size(800, 600);
            this.DoubleBuffered = true;

            // Отображаем начальное состояние
            CreateControls();
        }

        private void CreateControls()
        {
            this.Controls.Clear();

            // Отображаем карты игрока
            int x = 20, y = 300;
            foreach (var card in playerHand)
            {
                var pictureBox = CreateCardPictureBox(card, x, y);
                this.Controls.Add(pictureBox);
                x += 80;
            }

            // Отображаем карты крупье (первая открыта, вторая скрыта)
            x = 20;
            y = 50;
            var dealerCard = CreateCardPictureBox(dealerHand[0], x, y);
            this.Controls.Add(dealerCard);

            x += 80;
            var hiddenCard = CreateCardBackPictureBox(x, y);
            this.Controls.Add(hiddenCard);

            // Метка для очков игрока
            var playerScoreLabel = new Label
            {
                Text = $"Ваши очки: {GameLogic.CalculateScore(playerHand)}",
                Location = new Point(20, 420),
                Size = new Size(150, 20)
            };
            this.Controls.Add(playerScoreLabel);

            // Кнопки для действий
            if (playerTurn)
            {
                var hitButton = new Button
                {
                    Text = "Hit",
                    Location = new Point(20, 450),
                    Size = new Size(100, 30)
                };
                hitButton.Click += HitButton_Click;
                this.Controls.Add(hitButton);

                var standButton = new Button
                {
                    Text = "Stand",
                    Location = new Point(130, 450),
                    Size = new Size(100, 30)
                };
                standButton.Click += StandButton_Click;
                this.Controls.Add(standButton);
            }
        }

        private PictureBox CreateCardPictureBox(Card card, int x, int y)
        {
            string[] suits = { "♥", "♦", "♣", "♠" };
            string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            int cardRow = Array.IndexOf(suits, card.Suit); // Индекс строки (масть)
            int cardCol = Array.IndexOf(ranks, card.Rank); // Индекс столбца (ранг)

            int xe1 = cardCol * SizeBX + StartX;
            int ye1 = cardRow * SizeBY + StartY + OffsetY;
            Rectangle cardRect = new Rectangle(xe1, ye1, SizeX, SizeY);

            Bitmap singleCard = new Bitmap(SizeX, SizeY);
            using (Graphics g = Graphics.FromImage(singleCard))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.DrawImage(deckImage, new Rectangle(0, 0, SizeX, SizeY), cardRect, GraphicsUnit.Pixel);
            }

            return new PictureBox
            {
                Image = singleCard,
                Size = new Size(SizeX, SizeY),
                Location = new Point(x, y),
                SizeMode = PictureBoxSizeMode.Normal
            };
        }

        private PictureBox CreateCardBackPictureBox(int x, int y)
        {
            // Рубашка: строка 0, столбец 12
            int cardRow = 0;
            int cardCol = 12;

            int xe1 = cardCol * SizeBX + StartX;
            int ye1 = cardRow * SizeBY + StartY + OffsetY;
            Rectangle cardRect = new Rectangle(xe1, ye1, SizeX, SizeY);

            Bitmap cardBack = new Bitmap(SizeX, SizeY);
            using (Graphics g = Graphics.FromImage(cardBack))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.DrawImage(deckImage, new Rectangle(0, 0, SizeX, SizeY), cardRect, GraphicsUnit.Pixel);
            }

            return new PictureBox
            {
                Image = cardBack,
                Size = new Size(SizeX, SizeY),
                Location = new Point(x, y),
                SizeMode = PictureBoxSizeMode.Normal
            };
        }

        private void HitButton_Click(object sender, EventArgs e)
        {
            playerHand.Add(deck.PopLast());
            int playerScore = GameLogic.CalculateScore(playerHand);
            CreateControls();

            if (playerScore > 21)
            {
                playerTurn = false;
                ShowFinalResults();
                MessageBox.Show("Перебор! Вы проиграли!", "Игра окончена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EndGame();
            }
        }

        private void StandButton_Click(object sender, EventArgs e)
        {
            playerTurn = false;
            GameLogic.PlayCrupeTurn(deck, dealerHand);
            ShowFinalResults();
            string result = GameLogic.DetermineWinner(playerHand, dealerHand);
            MessageBox.Show(result, "Результат игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
            EndGame();
        }

        private void ShowFinalResults()
        {
            this.Controls.Clear();

            // Отображаем карты игрока
            int x = 20, y = 300;
            foreach (var card in playerHand)
            {
                var pictureBox = CreateCardPictureBox(card, x, y);
                this.Controls.Add(pictureBox);
                x += 80;
            }

            // Отображаем все карты крупье
            x = 20;
            y = 50;
            foreach (var card in dealerHand)
            {
                var pictureBox = CreateCardPictureBox(card, x, y);
                this.Controls.Add(pictureBox);
                x += 80;
            }

            // Метки с очками
            var playerScoreLabel = new Label
            {
                Text = $"Ваши очки: {GameLogic.CalculateScore(playerHand)}",
                Location = new Point(20, 420),
                Size = new Size(150, 20)
            };
            this.Controls.Add(playerScoreLabel);

            var dealerScoreLabel = new Label
            {
                Text = $"Очки крупье: {GameLogic.CalculateScore(dealerHand)}",
                Location = new Point(20, 20),
                Size = new Size(150, 20)
            };
            this.Controls.Add(dealerScoreLabel);
        }

        private void EndGame()
        {
            var newGameButton = new Button
            {
                Text = "Новая игра",
                Location = new Point(20, 450),
                Size = new Size(100, 30)
            };
            newGameButton.Click += (s, e) =>
            {
                InitializeGame();
            };
            this.Controls.Add(newGameButton);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                deckImage?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}