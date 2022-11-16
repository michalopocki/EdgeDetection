using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.Factory
{
    public class EdgeDetectorFactory : IEdgeDetectorFactory
    {
        private readonly IReadOnlyList<Type> _edgeDetectors;

        public EdgeDetectorFactory()
        {
            _edgeDetectors = GetAllTypesThatImplementInterface<IEdgeDetector>();
        }

        public IEdgeDetector Get(string name, IEdgeDetectorArgs args)
        {
            Type edgeDetector = _edgeDetectors.Where(x => x.Name.Contains(name)).First();

            return Activator.CreateInstance(edgeDetector, args) as IEdgeDetector
                                                ?? throw new InvalidOperationException();
        }

        public IReadOnlyList<string> GetAll()
        {
            IReadOnlyList<string> detectorsNames = _edgeDetectors.Select(x => EdgeDetectorBase.GetName(x)).ToList();
            return detectorsNames;
        }

        private IReadOnlyList<Type> GetAllTypesThatImplementInterface<T>()
        {
            var @interface = typeof(T);
            return @interface.IsInterface
                       ? AppDomain.CurrentDomain.GetAssemblies()
                             .SelectMany(assembly => assembly.GetTypes())
                             .Where(type => !type.IsInterface
                                         && !type.IsAbstract
                                         && type.GetInterfaces().Contains(@interface))
                             .ToList()
                       : new Type[] { };

        }
    }
}
