﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class MarrHildrethArgs : BaseArgs
    {
        public int KernelSize { get; set; }
        public double Sigma { get; set; }
        public MarrHildrethArgs(Bitmap? imageToProcess, bool isGrayscale, int kernelSize, double sigma) : base(imageToProcess, isGrayscale)
        {
            KernelSize = kernelSize;
            Sigma = sigma;
        }
    }
}
