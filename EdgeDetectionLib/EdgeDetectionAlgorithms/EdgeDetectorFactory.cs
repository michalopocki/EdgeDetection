﻿using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace EdgeDetectionLib.EdgeDetectionAlgorithms
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
        public IEdgeDetector Get(string name, IEdgeDetectorArgs args)
        {
            IEdgeDetector edgeDetector = _EdgeDetectors.Where(x => x.Name == name).First();

            return (IEdgeDetector)Activator.CreateInstance(edgeDetector.GetType(), args);
        }

    }
}
