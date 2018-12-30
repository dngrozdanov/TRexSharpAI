using System.Collections.Generic;
using System.Drawing;

namespace TRexSharp.Constants
{
    public static class TRexAssets
    {
        private const string CactusOneImagePath = "assets/cactusOne.png";
        private const string CactusTwoImagePath = "assets/cactusTwo.png";
        private const string CactusThreeImagePath = "assets/cactusThree.png";
        private const string CactusFourImagePath = "assets/cactusFour.png";
        private const string CactusFiveImagePath = "assets/cactusFive.png";
        private const string CactusSixImagePath = "assets/cactusSix.png";

        private const string TRexImagePath = "assets/trex.png";
        public static readonly string RestartBtnImagePath = "assets/restartBtn.png";

        public static readonly Bitmap CactusOneImage = new Bitmap(CactusOneImagePath);
        public static readonly Bitmap CactusTwoImage = new Bitmap(CactusTwoImagePath);
        public static readonly Bitmap CactusThreeImage = new Bitmap(CactusThreeImagePath);
        public static readonly Bitmap CactusFourImage = new Bitmap(CactusFourImagePath);
        public static readonly Bitmap CactusFiveImage = new Bitmap(CactusFiveImagePath);
        public static readonly Bitmap CactusSixImage = new Bitmap(CactusSixImagePath);

        // Used for Image Recognition (assets)
        public static readonly List<Bitmap> CollisionObjects = new List<Bitmap>
            {CactusOneImage, CactusTwoImage, CactusThreeImage, CactusFourImage, CactusFiveImage, CactusSixImage};

        public static readonly Bitmap TRexImage = new Bitmap(TRexImagePath);
        public static readonly Bitmap RestartBtnImage = new Bitmap(RestartBtnImagePath);
    }
}