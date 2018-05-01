using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections;

namespace Market.Extensions.DependencyInjection
{

    public class ForkedServiceCollection : IServiceCollection
    {
        private IServiceCollection _primary;
        private IEnumerable<IServiceCollection> _seconderies;

        public ForkedServiceCollection(IServiceCollection primary,IServiceCollection secondary) :
                this(primary, new[] {  secondary })
        {
            

        }

        public ForkedServiceCollection(IServiceCollection primary, IServiceCollection[] seconderies)
        {
            _primary = primary;
            _seconderies = seconderies;
        }

        public ServiceDescriptor this[int index]
        {
            get
            {
                return _primary[index];
            }

            set
            {
                _primary[index] = value;

                foreach (var s in _seconderies)
                    s.Add(value);
            }
        }

        public int Count
        {
            get
            {
                return _primary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _primary.IsReadOnly;
            }
        }

        public void Add(ServiceDescriptor item)
        {
            _primary.Add(item);

            foreach (var s in _seconderies)
                s.Add(item);
        }

        public void Clear()
        {
            _primary.Clear();

            foreach (var s in _seconderies)
                s.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return _primary.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _primary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _primary.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return _primary.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _primary.Insert(index, item);

            foreach (var s in _seconderies)
                s.Insert(s.Count, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            var b = _primary.Remove(item);

            foreach (var s in _seconderies)
                s.Remove(item);

            return b;
        }

        public void RemoveAt(int index)
        {
            var item = _primary[index];

            _primary.RemoveAt(index);

            foreach (var s in _seconderies)
                s.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _primary.GetEnumerator();
        }
    }
}