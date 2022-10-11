using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class EdgeDetectorFactory : IEdgeDetectorFactory
    {
        private readonly IReadOnlyList<IEdgeDetector> _EdgeDetectors;
        public EdgeDetectorFactory()
        {
            var calcType = typeof(IEdgeDetector);
            _EdgeDetectors = calcType
                             .Assembly
                             .ExportedTypes
                             .Where(x => calcType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                             .Select(x => { return Activator.CreateInstance(x); })
                             .Cast<IEdgeDetector>()
                             .OrderBy(x => x.Name)
                             .ToList();
        }
        public IReadOnlyList<IEdgeDetector> GetAll()
        {
            return _EdgeDetectors;
        }
        public IEdgeDetector Get(IEdgeDetector edgeDetector, Bitmap originalImage, bool isGrayscale)
        {
            object[] args = { originalImage, isGrayscale };

            return (IEdgeDetector)Activator.CreateInstance(edgeDetector.GetType(), args);
        }

    }
}
