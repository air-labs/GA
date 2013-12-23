using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AIRLab.CA;
using AIRLab.GeneticAlgorithms;

namespace AIRLab.Bulldozer
{
    public class LambdaCache<T>
    {
        private readonly IDictionary<string, Func<IList, T>> _delegateCache;
        private readonly IDictionary<string, int> _timestamps;

        private int _size;
        public int Size
        {
            get { return _size; }
            set 
            {
                if (value < 0) 
                    throw new InvalidOperationException("Negative cache size");
                _size = value;
            }
        }

        public LambdaCache():this(0)
        { } 

        public LambdaCache(int size)
        {
            Size = size;
            _delegateCache = new Dictionary<string, Func<IList, T>>();
            _timestamps = new Dictionary<string, int>();
        }

        public void Clear()
        {
            _delegateCache.Clear();
        }

        public Func<IList, T> GetLambda(TreeChromosome treeChromosome)
        {
            var key = treeChromosome.ToString();
            return _delegateCache.ContainsKey(key)
                       ? UpdateRecord(key)
                       : AddCacheRecord(key, treeChromosome);
        }

        private Func<IList, T> UpdateRecord(string key)
        {
            UpdateTimestamp(key);
            return _delegateCache[key];
        }

        private Func<IList, T> AddCacheRecord(string key, TreeChromosome treeChromosome)
        {
            if(Size != 0 && _delegateCache.Count >= Size)
                Enumerable.Range(0, _delegateCache.Count - Size + 1)
                          .ForEach(delegate{RemoveRecord();});
            UpdateTimestamp(key);
            return (_delegateCache[key] = treeChromosome.Tree.ComplileDelegate<T>());
        }

        private void RemoveRecord()
        {
            var minKey = _timestamps.HasMinimal(p => p.Value).Key;
            _delegateCache.Remove(minKey);
            _timestamps.Remove(minKey);
        }

        private void UpdateTimestamp(string key)
        {
            _timestamps[key] = _timestamps.Count == 0
                                   ? 0
                                   : _timestamps.Values.Max() + 1;
        }
    }
}