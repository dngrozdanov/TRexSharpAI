using System.Drawing;

namespace TRexSharp.Brain.Contracts
{
    public interface ITRexImageProcessor
    {
        Bitmap mainHaystack { get; set; }
        Bitmap collisionAreaTop { get; set; }
        Bitmap collisionAreaBottom { get; set; }
        Point? Find(Bitmap haystack, Bitmap needle);
        bool HasColor(int sourceX, int sourceY, Bitmap collisionArea, Color matchColor);
    }
}