using System.Collections.Generic;
using System.Linq;

namespace PhotoAlbum
{
    public class DualDictionary<TKey, TVal>
    {
        private Dictionary<TKey, TVal> _byKeysDict = new Dictionary<TKey, TVal>();
        private Dictionary<TVal, TKey> _byValsDict = new Dictionary<TVal, TKey>();

        public void Clear()
        {
            _byKeysDict.Clear();
            _byValsDict.Clear();
        }

        public void Add(TKey key, TVal val)
        {
            _byKeysDict.Add(key, val);
            _byValsDict.Add(val, key);
        }

        public void Add((TKey key, TVal val) pair) => Add(pair.key, pair.val);

        public void Add(IEnumerable<(TKey key, TVal val)> pairs) => pairs.ToList().ForEach(Add);

        public bool ContainsKey(TKey key) => _byKeysDict.ContainsKey(key);

        public bool ContainsVal(TVal val) => _byValsDict.ContainsKey(val);

        public (TKey key, TVal val) GetByKey(TKey key) => (key, _byKeysDict[key]);

        public (TKey key, TVal val) GetByVal(TVal val) => (_byValsDict[val], val);

        public void RemoveByKey(TKey key)
        {
            if (!ContainsKey(key)) return;
            _byValsDict.Remove(_byKeysDict[key]);
            _byKeysDict.Remove(key);
        }

        public void RemoveByVal(TVal val)
        {
            if (!ContainsVal(val)) return;
            _byKeysDict.Remove(_byValsDict[val]);
            _byValsDict.Remove(val);
        }
    }
}