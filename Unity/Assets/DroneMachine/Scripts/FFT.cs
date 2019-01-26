/* 
 * Free FFT and convolution (C++)
 * 
 * Copyright (c) 2014 Project Nayuki
 * https://www.nayuki.io/page/free-small-fft-in-multiple-languages
 * 
 * (MIT License)
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * - The above copyright notice and this permission notice shall be included in
 *   all copies or substantial portions of the Software.
 * - The Software is provided "as is", without warranty of any kind, express or
 *   implied, including but not limited to the warranties of merchantability,
 *   fitness for a particular purpose and noninfringement. In no event shall the
 *   authors or copyright holders be liable for any claim, damages or other
 *   liability, whether in an action of contract, tort or otherwise, arising from,
 *   out of or in connection with the Software or the use or other dealings in the
 *   Software.
 */

/*
Minor changes made to port this to C# and work the way I needed it to. -CH
*/
using System;
using UnityEngine;

namespace DerelictComputer.DroneMachine
{
    public class FFT
    {
        public static void Forward(double[] real, double[] imag)
        {
            if (real.Length != imag.Length)
            {
                Debug.LogError("FFT: Input lengths do not match.");
                return;
            }

            int n = real.Length;
            int levels;
            {
                int temp = n;
                levels = 0;

                while (temp > 1)
                {
                    levels++;
                    temp >>= 1;
                }

                if (1 << levels != n)
                {
                    Debug.LogError("FFT: Length is not a power of 2");
                    return;
                }
            }

            // Trignometric tables
            double[] cosTable = new double[n/2];
            double[] sinTable = new double[n/2];
            for (int i = 0; i < n / 2; i++)
            {
                cosTable[i] = Math.Cos(2 * Math.PI * i / n);
                sinTable[i] = Math.Sin(2 * Math.PI * i / n);
            }

            // Bit-reversed addressing permutation
            for (int i = 0; i < n; i++)
            {
                int j = ReverseBits(i, levels);
                if (j > i)
                {
                    double temp = real[i];
                    real[i] = real[j];
                    real[j] = temp;
                    temp = imag[i];
                    imag[i] = imag[j];
                    imag[j] = temp;
                }
            }

            // Cooley-Tukey decimation-in-time radix-2 FFT
            for (int size = 2; size <= n; size *= 2)
            {
                int halfsize = size / 2;
                int tablestep = n / size;
                for (int i = 0; i < n; i += size)
                {
                    for (int j = i, k = 0; j < i + halfsize; j++, k += tablestep)
                    {
                        double tpre = real[j + halfsize] * cosTable[k] + imag[j + halfsize] * sinTable[k];
                        double tpim = -real[j + halfsize] * sinTable[k] + imag[j + halfsize] * cosTable[k];
                        real[j + halfsize] = real[j] - tpre;
                        imag[j + halfsize] = imag[j] - tpim;
                        real[j] += tpre;
                        imag[j] += tpim;
                    }
                }
                if (size == n)  // Prevent overflow in 'size *= 2'
                    break;
            }
        }

        public static void Inverse(double[] real, double[] imag)
        {
            Forward(imag, real);
        }

        static int ReverseBits(int x, int n)
        {
            int result = 0;
            int i;
            for (i = 0; i < n; i++, x >>= 1)
                result = (result << 1) | (x & 1);
            return result;
        } 
    }
}