﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public abstract class BaseArgs : IEdgeDetectorArgs
    {
        public Bitmap? ImageToProcess { get; set; }
        public bool IsGrayscale { get; set; }
        public BaseArgs(Bitmap? imageToProcess, bool isGrayscale)
        {
            ImageToProcess = imageToProcess;
            IsGrayscale = isGrayscale;
        }
    }
}