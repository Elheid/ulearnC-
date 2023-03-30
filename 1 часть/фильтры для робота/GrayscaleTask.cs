namespace Recognizer
{
    public static class GrayscaleTask
    {
        const double CoefficientForRed = 0.299;
        const double CoefficientForGreen = 0.587;
        const double CoefficientForBlue = 0.114;
        const int WhiteColour = 255;
        public static double[,] ToGrayscale(Pixel[,] original)
        {
            var pixelsInX = original.GetLength(0);
            var pixelsInY = original.GetLength(1);
            var grayscale = new double[pixelsInX, pixelsInY];
            for (int x = 0; x < pixelsInX; x++)
            {
                for (int y = 0; y < pixelsInY; y++)
                {
                    Pixel paint = original[x, y];
                    var newRed = CoefficientForRed * paint.R;
                    var newGreen = CoefficientForGreen * paint.G;
                    var newBlue = CoefficientForBlue * paint.B;
                    var formulaTonesOfGrey = (newRed + newGreen + newBlue) / WhiteColour;
                    grayscale[x, y] = formulaTonesOfGrey;
                }
            }
            return grayscale;
        }
    }
}

/* 
 * Переведите изображение в серую гамму.
 * 
 * original[x, y] - массив пикселей с координатами x, y. 
 * Каждый канал R,G,B лежит в диапазоне от 0 до 255.
 * 
 * Получившийся массив должен иметь те же размеры, 
 * grayscale[x, y] - яркость пикселя (x,y) в диапазоне от 0.0 до 1.0
 *
 * Используйте формулу:
 * Яркость = (0.299*R + 0.587*G + 0.114*B) / 255
 * 
 * Почему формула именно такая — читайте в википедии 
 * http://ru.wikipedia.org/wiki/Оттенки_серого
*/


