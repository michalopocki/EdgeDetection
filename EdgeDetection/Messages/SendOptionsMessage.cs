using EdgeDetectionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Messages
{
    public record SendOptionsMessage(DetectionParameters Parameters);
}
