using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoFiremark.Utilities
{
    public static class PhotoHider
    {

        public static Image<Rgb, byte> Embed(this Image<Rgb, byte> source, Image<Rgb, byte> destination)
        {
            var result = source.Copy() ;

            var indexCounter = 0;
            for(int i = 0; i < destination.Rows; i++)
            {
                for(int j = 0; j < destination.Cols; j++)
                {
                    var (row, column) = indexCounter++.GetLargePhotoIndex();
                    var currentPixel = destination[i, j];

                    result.ModifyPhotoUsingLSB(row, column, currentPixel);

                }
            }

            return result;
        }

        public static void ModifyPhotoUsingLSB(this Image<Rgb, byte> source, int row, int column, Rgb pixelColor)
        {
            
            var (rBinary, gBinary, bBinary) = pixelColor.BinaryRepresentationOfColor();
            int newR, newG, newB; bool isZero;

            for (int k = 0; k < 8; k++)
            {
                var targetPixel = source[row, column];
                byte r = (byte)targetPixel.Red;
                byte g = (byte)targetPixel.Green;
                byte b = (byte)targetPixel.Blue;

                // Red
                isZero = rBinary[7 - k] == '0';
                newR = (isZero) ? r.ResetLSB() : r.SetLSB();

                // Green
                isZero = gBinary[7 - k] == '0';
                newG = (isZero) ? g.ResetLSB() : g.SetLSB();

                // Blue
                isZero = bBinary[7 - k] == '0';
                newB = (isZero) ? b.ResetLSB() : b.SetLSB();

                source[row, column] = new Rgb(newR, newG, newB);

                IncrementPhotoIndex(ref row, ref column, 1000);
            }
        }

        public static Image<Rgb, byte> ExtractSecretPhoto(this Image<Rgb, byte> largeImage){
            var photo = new Image<Rgb, byte>(350, 350);

            byte counter = 0; bool flagToBreak = false;
            int rCounter = 0, gCounter = 0, bCounter = 0, row = 0, column = 0;

            for(int i = 0; i < largeImage.Rows; i++)
            {
                for(int j = 0; j < largeImage.Cols; j++)
                {
                    var pixel = largeImage[i, j];
                    var (r, g, b) = pixel.GetRgb();

                    // Red
                    rCounter += (int)Math.Pow(2, counter) * r.Lsb();

                    // Green
                    gCounter += (int)Math.Pow(2, counter) * g.Lsb();

                    // Blue
                    bCounter += (int)Math.Pow(2, counter) * b.Lsb();

                    counter++;
                    if( counter == 8)
                    {
                        photo[row, column] = new Rgb(rCounter, gCounter, bCounter);
                        rCounter = gCounter = bCounter = counter = 0; // Reset All
                        IncrementPhotoIndex(ref row, ref column, 350);
                        if (row > 349)
                        {
                            flagToBreak = true;
                            break;
                        }
                            
                    }
                }

                if (flagToBreak) break;
            }

            return photo;
        }

        public static (int row, int column) GetLargePhotoIndex(this int pixelIndex)
        {
            int newIndex = pixelIndex * 8;
            return (newIndex / 1000, newIndex % 1000);
        }

        public static (byte r, byte g, byte b) GetRgb(this Rgb rgb)
        {
            
            return ((byte)rgb.Red, (byte)rgb.Green, (byte)rgb.Blue);
        }

        public static byte SetLSB(this byte b)
        {
            if (b % 2 == 0)
                return (byte)(b + 1);
            return b;
        }

        public static byte ResetLSB(this byte b)
        {
            if (b % 2 != 0)
                return (byte)(b - 1);
            return b;
        }

        public static bool IsZeroLSB(this byte d)
        {
            return (d % 2 == 0);
        }

        public static int Lsb(this byte d)
        {
            return (d % 2 == 0) ? 0 : 1;
        }

        public static (string rBinary, string gBinary, string bBinary) BinaryRepresentationOfColor(this Rgb rgb)
        {
            var (r, g, b) = rgb.GetRgb();
            return (
                Convert.ToString(r, 2).PadLeft(8, '0'),
                Convert.ToString(g, 2).PadLeft(8, '0'),
                Convert.ToString(b, 2).PadLeft(8, '0')
                );
        }

        public static void IncrementPhotoIndex(ref int row, ref int column, int max)
        {
            column++;

            if(column > max - 1)
            {
                row++;
                column = 0;
            }
        }

    }
}
