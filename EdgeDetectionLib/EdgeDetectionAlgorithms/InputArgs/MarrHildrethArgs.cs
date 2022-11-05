﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class MarrHildrethArgs : BaseArgs, IMarrHildrethArgs
    {
        public int KernelSize { get; set; }
        public double Sigma { get; set; }
        public MarrHildrethArgs(Bitmap? imageToProcess, int kernelSize, double sigma) : base(imageToProcess)
        {
            KernelSize = kernelSize;
            Sigma = sigma;
        }
    }
}
