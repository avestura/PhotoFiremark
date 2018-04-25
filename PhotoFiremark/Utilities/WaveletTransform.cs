using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoFiremark.Utilities
{
    public static partial class WaveletTransform
    {

        public enum WaveletMethod
        {
            Daub4, Daub10, Haar
        }

        public enum WaveletDirection
        {
            Horizontal, Vertical
        }

        public enum PassFilter
        {
            High, Low
        }

        public static double[] LowPass4  { get; } = new double[] { -0.1294, 0.2241, 0.8365, 0.4830 };
        public static double[] HighPass4 { get; } = new double[] { -0.4830, 0.8365, -0.2241, -0.1294 };

        public static Image<Rgb, byte> Transform(this Image<Rgb, byte> image, int n, int m, WaveletDirection direction, WaveletMethod waveletMethod = WaveletMethod.Daub4)
        {
            if(direction == WaveletDirection.Horizontal)
            {
                return image.TransformHorizontal(n, m, direction, waveletMethod);
            }
            else
            {
                return image.TransformVertical(n, m, direction, waveletMethod);
            }
        }

        private static Image<Rgb, byte> TransformHorizontal(this Image<Rgb, byte> image, int n, int m, WaveletDirection direction, WaveletMethod waveletMethod = WaveletMethod.Daub4)
        {
            int i, j, jj;
            var outImage = new Image<Rgb, byte>(n, m);

            for (i = 0; i < n; i++)
            {
                for (j = 0, jj = 0; jj < m; j++, jj += 2)
                {
                    var (rL, gL, bL) = image.NewColorValue(i, jj, PassFilter.Low, WaveletDirection.Horizontal);
                    outImage[i, j] = new Rgb(rL, gL, bL);

                    var (rH, gH, bH) = image.NewColorValue(i, jj, PassFilter.High, WaveletDirection.Horizontal);
                    outImage[i, j + m / 2] = new Rgb(rH, gH, bH);
                }
            }

            return outImage;
        }

        private static Image<Rgb, byte> TransformVertical(
            this Image<Rgb, byte> image,
            int n,
            int m,
            WaveletDirection direction,
            WaveletMethod waveletMethod = WaveletMethod.Daub4)
        {
            int i, j, ii;
            var outImage = new Image<Rgb, byte>(n, m);

            for (i = 0, ii = 0; i < n; i+= 2)
            {
                for (j = 0; j < m; j++)
                {
                    var (rL, gL, bL) = image.NewColorValue(ii, j, PassFilter.Low, WaveletDirection.Vertical);
                    outImage[i, j] = new Rgb(rL, gL, bL);

                    var (rH, gH, bH) = image.NewColorValue(ii, j, PassFilter.High, WaveletDirection.Vertical);
                    outImage[i + n/2, j] = new Rgb(rH, gH, bH);
                }
            }

            return outImage;
        }

        public static (byte, byte, byte) NewColorValue(
            this Image<Rgb, byte> image,
            int rowIndex,
            int columnIndex,
            PassFilter passFilter,
            WaveletDirection direction)
        {
            if(direction == WaveletDirection.Horizontal)
            {
                return image.NewColorValueHorizontal(rowIndex, columnIndex, passFilter);
            }
            else
            {
                return image.NewColorValueVertical(rowIndex, columnIndex, passFilter);
            }
        }

        private static (byte, byte, byte) NewColorValueHorizontal(
            this Image<Rgb, byte> image,
            int i,
            int jj,
            PassFilter passFilter)
        {
            if (passFilter == PassFilter.High)
            {
                var red =
                     image[i, jj].Red * HighPass4[0] +
                     image[i, jj].Red * HighPass4[1] +
                     image[i, jj].Red * HighPass4[2] +
                     image[i, jj].Red * HighPass4[3];

                var green =
                     image[i, jj].Green * HighPass4[0] +
                     image[i, jj].Green * HighPass4[1] +
                     image[i, jj].Green * HighPass4[2] +
                     image[i, jj].Green * HighPass4[3];

                var blue =
                     image[i, jj].Blue * HighPass4[0] +
                     image[i, jj].Blue * HighPass4[1] +
                     image[i, jj].Blue * HighPass4[2] +
                     image[i, jj].Blue * HighPass4[3];

                return ((byte)red, (byte)green, (byte)blue);
            }
            else
            {
                var red =
                    image[i, jj].Red * LowPass4[0] +
                    image[i, jj].Red * LowPass4[1] +
                    image[i, jj].Red * LowPass4[2] +
                    image[i, jj].Red * LowPass4[3];

                var green =
                     image[i, jj].Green * LowPass4[0] +
                     image[i, jj].Green * LowPass4[1] +
                     image[i, jj].Green * LowPass4[2] +
                     image[i, jj].Green * LowPass4[3];

                var blue =
                     image[i, jj].Blue * LowPass4[0] +
                     image[i, jj].Blue * LowPass4[1] +
                     image[i, jj].Blue * LowPass4[2] +
                     image[i, jj].Blue * LowPass4[3];

                return ((byte)red, (byte)green, (byte)blue);
            }
        }

        private static (byte, byte, byte) NewColorValueVertical(
            this Image<Rgb, byte> image,
            int ii,
            int j,
            PassFilter passFilter)
        {
            if (passFilter == PassFilter.High)
            {
                var red =
                     image[ii + 0, j].Red * HighPass4[0] +
                     image[ii + 1, j].Red * HighPass4[1] +
                     image[ii + 2, j].Red * HighPass4[2] +
                     image[ii + 3, j].Red * HighPass4[3];

                var green =
                     image[ii + 0, j].Green * HighPass4[0] +
                     image[ii + 1, j].Green * HighPass4[1] +
                     image[ii + 2, j].Green * HighPass4[2] +
                     image[ii + 3, j].Green * HighPass4[3];

                var blue =
                     image[ii + 0, j].Blue * HighPass4[0] +
                     image[ii + 1, j].Blue * HighPass4[1] +
                     image[ii + 2, j].Blue * HighPass4[2] +
                     image[ii + 3, j].Blue * HighPass4[3];

                return ((byte)red, (byte)green, (byte)blue);
            }
            else
            {
                var red =
                    image[ii + 0, j].Red * LowPass4[0] +
                    image[ii + 1, j].Red * LowPass4[1] +
                    image[ii + 2, j].Red * LowPass4[2] +
                    image[ii + 3, j].Red * LowPass4[3];

                var green =
                     image[ii + 0, j].Green * LowPass4[0] +
                     image[ii + 1, j].Green * LowPass4[1] +
                     image[ii + 2, j].Green * LowPass4[2] +
                     image[ii + 3, j].Green * LowPass4[3];

                var blue =
                     image[ii + 0, j].Blue * LowPass4[0] +
                     image[ii + 1, j].Blue * LowPass4[1] +
                     image[ii + 2, j].Blue * LowPass4[2] +
                     image[ii + 3, j].Blue * LowPass4[3];

                return ((byte)red, (byte)green, (byte)blue);
            }
        }

    }
}
