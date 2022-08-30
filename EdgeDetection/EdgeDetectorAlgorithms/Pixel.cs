using System;
using System.Drawing;

namespace EdgeDetection.EdgeDetectorAlgorithms
{
    public struct Pixel
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public Pixel(double r, double g, double b)
        {
            R = r; G = g; B = b;
        }
        public static implicit operator Pixel(Color color)
        {
            return new Pixel(color.R, color.G, color.B);
        }
        public static Pixel operator *(Pixel pixel1, Pixel pixel2)
        {
            return new Pixel(pixel1.R * pixel2.R, pixel1.G * pixel2.G, pixel1.B * pixel2.B);
        }
        public static Pixel operator *(Pixel pixel1, double value)
        {
            return new Pixel(pixel1.R * value, pixel1.G * value, pixel1.B * value);
        }
        public static Pixel operator +(Pixel pixel1, Pixel pixel2)
        {
            return new Pixel(pixel1.R + pixel2.R, pixel1.G + pixel2.G, pixel1.B + pixel2.B);
        }
        public static Pixel operator -(Pixel pixel1, int value)
        {
            return new Pixel(pixel1.R - value, pixel1.G - value, pixel1.B - value);
        }
        public static Pixel Sqrt(Pixel pixel)
        {
            return new Pixel(Math.Sqrt(pixel.R), Math.Sqrt(pixel.G), Math.Sqrt(pixel.G));
        }
        public static Pixel Abs(Pixel pixel)
        {
            return new Pixel(Math.Abs(pixel.R), Math.Abs(pixel.G), Math.Abs(pixel.G));
        }
        public static Pixel Greyscale(Pixel pixel)
        {
            double intensity = 0.2989 * pixel.R + 0.5870 * pixel.G + 0.1140 * pixel.B;
            return new Pixel(intensity, intensity, intensity);
        }
        public static Pixel Thresholing(Pixel pixel, int threshold)
        {
            Pixel thresholdingImg = new Pixel();
            thresholdingImg.R = pixel.R <= threshold ? 0 : 255;
            thresholdingImg.G = pixel.G <= threshold ? 0 : 255;
            thresholdingImg.B = pixel.B <= threshold ? 0 : 255;
            return thresholdingImg;
        }
    }
}
