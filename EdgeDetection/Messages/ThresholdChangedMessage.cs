using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Messages
{
    public record ThresholdChangedMessage(int Threshold1, int Threshold2, bool Threshold2Visibility = false);
}
