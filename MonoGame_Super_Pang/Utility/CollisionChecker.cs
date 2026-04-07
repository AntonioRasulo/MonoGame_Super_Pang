using Microsoft.Xna.Framework;
using MonoGameLibrary;
using System;

namespace MonoGame_Super_Pang.Utility;

public class CollisionChecker
{
    public static bool areIntersecting(Circle circle, Rectangle rectangle)
    {
        int distanceX = Math.Abs(circle.X - rectangle.Center.X);
        int distanceY = Math.Abs(circle.Y - rectangle.Center.Y);

        float halfRectWidth = rectangle.Width * 0.5f;
        float halfRectHeight = rectangle.Height * 0.5f;

        if((distanceX > (halfRectWidth + circle.Radius)) ||
           (distanceY > (halfRectHeight + circle.Radius)))
        {
            return false;
        }

        if(distanceX <= halfRectWidth ||
           distanceY <= halfRectHeight)
        {
            return true;
        }
            double cornerDistanceSquare = Math.Pow(distanceX-halfRectWidth, 2) + Math.Pow(distanceY-halfRectHeight, 2);

        return cornerDistanceSquare <= Math.Pow(circle.Radius, 2);
    }

}