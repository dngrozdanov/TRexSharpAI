using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using TRexSharp.Brain.Contracts;

namespace TRexSharp.Brain
{
    public class TRexImageProcessor : ITRexImageProcessor
    {
        private Bitmap collisionareaBottom;
        private Bitmap collisionareaTop;
        private Bitmap mainhaystack;

        public Bitmap mainHaystack
        {
            get => mainhaystack;
            set
            {
                mainhaystack?.Dispose();
                mainhaystack = value;
            }
        }

        public Bitmap collisionAreaTop
        {
            get => collisionareaTop;
            set
            {
                collisionareaTop?.Dispose();
                collisionareaTop = value;
            }
        }

        public Bitmap collisionAreaBottom
        {
            get => collisionareaBottom;
            set
            {
                collisionareaBottom?.Dispose();
                collisionareaBottom = value;
            }
        }

        public bool HasColor(int sourceX, int sourceY, Bitmap collisionArea, Color matchColor)
        {
            using (var snapshot = new Bitmap(collisionArea.Width, collisionArea.Height, PixelFormat.Format24bppRgb))
            using (var gph = Graphics.FromImage(snapshot))
            {
                gph.CopyFromScreen(sourceX, sourceY, 0, 0, collisionArea.Size, CopyPixelOperation.SourceCopy);
                for (var i = 0; i < collisionArea.Height; i++)
                for (var j = 0; j < collisionArea.Width; j++)
                    if (snapshot.GetPixel(j, i) == matchColor)
                        return true;
                return false;
            }
        }

        public Point? Find(Bitmap haystack, Bitmap needle)
        {
            if (null == haystack || null == needle) return null;
            if (haystack.Width < needle.Width || haystack.Height < needle.Height) return null;

            var haystackArray = GetPixelArray(haystack);
            var needleArray = GetPixelArray(needle);

            foreach (var firstLineMatchPoint in FindMatch(haystackArray.Take(haystack.Height - needle.Height),
                needleArray[0]))
                if (IsNeedlePresentAtLocation(haystackArray, needleArray, firstLineMatchPoint, 1))
                    return firstLineMatchPoint;

            return null;
        }

        private int[][] GetPixelArray(Bitmap bitmap)
        {
            var result = new int[bitmap.Height][];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            for (var y = 0; y < bitmap.Height; ++y)
            {
                result[y] = new int[bitmap.Width];
                Marshal.Copy(bitmapData.Scan0 + y * bitmapData.Stride, result[y], 0, result[y].Length);
            }

            bitmap.UnlockBits(bitmapData);

            return result;
        }

        private IEnumerable<Point> FindMatch(IEnumerable<int[]> haystackLines, int[] needleLine)
        {
            var y = 0;
            foreach (var haystackLine in haystackLines)
            {
                for (int x = 0, n = haystackLine.Length - needleLine.Length; x < n; ++x)
                    if (ContainSameElements(haystackLine, x, needleLine, 0, needleLine.Length))
                        yield return new Point(x, y);
                y += 1;
            }
        }

        private bool ContainSameElements(int[] first, int firstStart, int[] second, int secondStart, int length)
        {
            for (var i = 0; i < length; ++i)
                if (first[i + firstStart] != second[i + secondStart])
                    return false;
            return true;
        }

        private bool IsNeedlePresentAtLocation(int[][] haystack, int[][] needle, Point point, int alreadyVerified)
        {
            for (var y = alreadyVerified; y < needle.Length; ++y)
                if (!ContainSameElements(haystack[y + point.Y], point.X, needle[y], 0, needle.Length))
                    return false;
            return true;
        }
    }
}