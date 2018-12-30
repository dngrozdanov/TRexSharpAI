using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using WindowsInput;
using TRexSharp.Brain;
using TRexSharp.Brain.Contracts;
using TRexSharp.Constants;

namespace TRexSharp.Core
{
    internal class TRexBrain
    {
        private readonly ITRexImageProcessor imageProcessor;
        private readonly Timer increasePredictionTimer;
        private readonly IInputSimulator inputSimulator;
        private readonly TRex TRex;
        private bool collisionBottom;

        private bool collisionTop;
        private int predictionX = 60;

        public TRexBrain()
        {
            Console.WriteLine("Press any key to boot up!");
            Console.ReadKey();
            Console.WriteLine("=== Brain has been booted ===");
            imageProcessor = new TRexImageProcessor();
            inputSimulator = new InputSimulator();
            TRex = new TRex(inputSimulator);
            increasePredictionTimer = new Timer(1000);
            Play();
        }

        private void IncreasePredictionTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            // Make it so it changes dynamically @eXtraGoZ
            predictionX += 3;
            Console.WriteLine($"PredictionX updated: {predictionX}");
        }

        public void Play()
        {
            increasePredictionTimer.Start();
            increasePredictionTimer.Elapsed += IncreasePredictionTimerOnElapsed;

            while (true)
            {
                UpdateCanvas();
                IncomingCollisionTop();
                IncomingCollisionBottom();

                if (collisionTop && collisionBottom)
                {
                    if (TRex.IsDucking())
                    {
                        Console.WriteLine("TRex is ducking, releasing duck key!");
                        TRex.ReleaseDuck();
                    }

                    Console.WriteLine("Collision Found -> Long Jumping!");
                    TRex.LongJump();
                }
                else if (collisionBottom && !collisionTop)
                {
                    if (TRex.IsDucking())
                    {
                        Console.WriteLine("TRex is ducking, releasing duck key!");
                        TRex.ReleaseDuck();
                    }

                    Console.WriteLine("Collision Found -> Short Jumping!");
                    TRex.ShortJump();
                }
                else if (collisionTop && !collisionBottom)
                {
                    Console.WriteLine("Collision Found -> Ducking!");
                    TRex.HoldDuck();
                }
            }
        }

        // Unfinished I believe, after the changes...
        private void RestartGame()
        {
            var btnLocation = imageProcessor.Find(imageProcessor.mainHaystack, TRexAssets.RestartBtnImage);
            if (btnLocation == null)
            {
                Console.WriteLine("Restart Button Not Found!");
                return;
            }

            Helpers.SetCursorPos(btnLocation.Value.X, btnLocation.Value.Y);
            inputSimulator.Mouse.LeftButtonClick();
        }

        private void IncomingCollisionTop()
        {
            // 239 = Ducked front face
            // 222 = Stand up face
            collisionTop =
                imageProcessor.HasColor(222, 355, imageProcessor.collisionAreaTop, Color.FromArgb(83, 83, 83));
        }

        private void IncomingCollisionBottom()
        {
            // 239 = Ducked front face
            // 222 = Stand up face
            collisionBottom = imageProcessor.HasColor(TRex.IsDucking() ? 239 : 222, 388,
                imageProcessor.collisionAreaBottom, Color.FromArgb(83, 83, 83));
        }

        // Coded by ExTraGoZ - Evading Memory Leaks & Using Safe Casts (Could make it with using's instead)
        // Values are hardcoded, might not work as intended on other resolutions.
        private void UpdateCanvas()
        {
            var mainWindow = new Bitmap(958, 519, PixelFormat.Format32bppArgb);
            var mainGraphics = Graphics.FromImage(mainWindow);
            mainGraphics.CopyFromScreen(0, 0, 0, 0, new Size(958, 519), CopyPixelOperation.SourceCopy);
            imageProcessor.mainHaystack = mainWindow.Clone() as Bitmap;
            mainGraphics?.Dispose();
            mainWindow?.Dispose();

            var collisionPredictionTop = new Bitmap(predictionX, 32, PixelFormat.Format32bppArgb);
            var collisionGraphicsTop = Graphics.FromImage(collisionPredictionTop);
            collisionGraphicsTop.CopyFromScreen(222, 355, 0, 0, collisionPredictionTop.Size,
                CopyPixelOperation.SourceCopy);
            imageProcessor.collisionAreaTop = collisionPredictionTop.Clone() as Bitmap;
            collisionGraphicsTop?.Dispose();
            collisionPredictionTop?.Dispose();

            var collisionPredictionBottom = new Bitmap(predictionX, 13, PixelFormat.Format32bppArgb);
            var collisionGraphicsBottom = Graphics.FromImage(collisionPredictionBottom);
            collisionGraphicsBottom.CopyFromScreen(TRex.IsDucking() ? 239 : 222, 388, 0, 0,
                collisionPredictionBottom.Size, CopyPixelOperation.SourceCopy);
            imageProcessor.collisionAreaBottom = collisionPredictionBottom.Clone() as Bitmap;
            collisionGraphicsBottom?.Dispose();
            collisionPredictionBottom?.Dispose();
        }
    }
}