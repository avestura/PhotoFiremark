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
        // TODO: Change this documentation if source or destination photo changed from fixed size to generic size
        /// <summary>
        /// Embeds and hides a photo in another photo
        /// </summary>
        /// <param name="source">Source image as 1000x1000 file</param>
        /// <param name="destination">The secret image as 350x350 file</param>
        /// <returns>A large photo that has a hidden photo in it</returns>
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

        /// <summary>
        /// Modify least significant bit of a large photo to put smaller photos in it
        /// </summary>
        /// <param name="source">Source photo to put another image in it</param>
        /// <param name="row">Start row of large photo for starting modification</param>
        /// <param name="column">Start column of large photo for starting modification</param>
        /// <param name="pixelColor">The color of pixel in secret photo</param>
        private static void ModifyPhotoUsingLSB(this Image<Rgb, byte> source, int row, int column, Rgb pixelColor)
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

        /// <summary>
        /// Extracts a secret photo from a large photo
        /// </summary>
        /// <param name="largeImage">The large photo to extract</param>
        /// <returns>Returns the secret image</returns>
        public static Image<Rgb, byte> ExtractSecretPhoto(this Image<Rgb, byte> largeImage) {
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

        /// <summary>
        /// Converts an integer address to index of pixel in photo matrix
        /// </summary>
        /// <param name="pixelIndex">Index in integer</param>
        /// <returns>Row and Column of pixel in matrix</returns>
        public static (int row, int column) GetLargePhotoIndex(this int pixelIndex)
        {
            int newIndex = pixelIndex * 8;
            return (newIndex / 1000, newIndex % 1000);
        }

        /// <summary>
        /// Converts an <see cref="Rgb"/> to three separated bytes as a tuple (r, g, b)
        /// </summary>
        /// <param name="rgb">Target <see cref="Rgb"/></param>
        /// <returns>A tuple of literal</returns>
        public static (byte r, byte g, byte b) GetRgb(this Rgb rgb)
        {
            
            return ((byte)rgb.Red, (byte)rgb.Green, (byte)rgb.Blue);
        }

        /// <summary>
        /// Sets least significant bit of a byte
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte SetLSB(this byte b)
        {
            if (b % 2 == 0)
                return (byte)(b + 1);
            return b;
        }

        /// <summary>
        /// Resets least significant bit of a byte
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte ResetLSB(this byte b)
        {
            if (b % 2 != 0)
                return (byte)(b - 1);
            return b;
        }

        /// <summary>
        /// Is least significant bit of byte zero?
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsZeroLSB(this byte d)
        {
            return (d % 2 == 0);
        }

        /// <summary>
        /// Gets least significant bit of byte
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Least significant bit</returns>
        public static int Lsb(this byte d)
        {
            return (d % 2 == 0) ? 0 : 1;
        }

        /// <summary>
        /// Gets 8-bit binary representation of a <see cref="Rgb"/> as string
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static (string rBinary, string gBinary, string bBinary) BinaryRepresentationOfColor(this Rgb rgb)
        {
            var (r, g, b) = rgb.GetRgb();
            return (
                Convert.ToString(r, 2).PadLeft(8, '0'),
                Convert.ToString(g, 2).PadLeft(8, '0'),
                Convert.ToString(b, 2).PadLeft(8, '0')
                );
        }

        /// <summary>
        /// Increments sqaure-matrix cell index
        /// </summary>
        /// <param name="row">Current row of matrix</param>
        /// <param name="column">Current column of matrix</param>
        /// <param name="max">Resoloution of matrix</param>
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
