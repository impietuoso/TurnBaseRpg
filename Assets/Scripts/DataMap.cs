using System.Collections.Generic;

namespace DefaultNamespace {
    public class DataMap {
        private Dictionary<object, object> _data = new ();

        public T Get<T>() => _data.TryGetValue(typeof(T), out var v) ? (T)v : default;
        public void Set<T>(T value) => _data[typeof(T)] = value;

        public T GetOrCreate<T>() where T : new() {
            if (_data.TryGetValue(typeof(T), out var v)) return (T)v;
            return (T)(_data[typeof(T)] = new T());
        }

        public bool TryGet<T>(out T value) {
            if (_data.TryGetValue(typeof(T), out var v)) {
                value = (T)v;
                return true;
            }
            value = default;
            return false;
        }
    }
}