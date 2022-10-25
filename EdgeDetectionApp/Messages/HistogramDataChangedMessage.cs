using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Messages
{
    public record HistogramDataChangedMessage(Bitmap Bitmap);
}
