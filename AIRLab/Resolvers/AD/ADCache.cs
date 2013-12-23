using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace AIRLab.Thornado
{
    public class ADCache
    {
        Dictionary<FieldAddress, Dictionary<ADSpecification, object>> data = new Dictionary<FieldAddress, Dictionary<ADSpecification, object>>();
        Dictionary<FieldAddress, Dictionary<ADSpecification, object>> starredData = new Dictionary<FieldAddress,Dictionary<ADSpecification,object>>();



        public bool Has(FieldAddress address, ADSpecification spec)
        {
            if (data.ContainsKey(address))
                if (data[address].ContainsKey(spec))
                    return true;
            if (starredData.ContainsKey(address))
                if (starredData[address].ContainsKey(spec))
                    return true;
            return false;
        }

        public object Get(FieldAddress address, ADSpecification spec)
        {
            if (data.ContainsKey(address))
                if (data[address].ContainsKey(spec))
                   return data[address][spec];
            if (starredData.ContainsKey(address))
                if (starredData[address].ContainsKey(spec))
                    return starredData[address][spec];
            throw new Exception(string.Format("No AD of spec {1} is assotiated with {0}", address, spec));
        }

        public Q Get<Q>(FieldAddress address, ADSpecification<Q> spec)
        {
            return (Q)Get(address, (ADSpecification)spec);
        }

        public object GetOrDefault(FieldAddress address, ADSpecification spec)
        {
            if (!Has(address, spec)) return null;
            return Get(address, spec);
        }

        public Q GetOrDefault<Q>(FieldAddress address, ADSpecification<Q> spec)
        {
            if (!Has(address, spec)) return default(Q);
            return Get<Q>(address, spec);
        }

        IEnumerable<FieldAddress> GetStarMatches(FieldAddress address)
        {
            foreach (var e in starredData)
            {
                var stored = e.Key.Elements.ToArray();
                var given = address.Elements.ToArray();
                if (stored.Length != given.Length) continue;
                bool fail = false;
                for (int i = 0; i < stored.Length; i++)
                    if (stored[i] != "*" && stored[i] != given[i])
                    {
                        fail = true;
                        break;
                    }
                if (fail) continue;
                yield return e.Key;
            }
        }
        public IEnumerable<KeyValuePair<ADSpecification, object>> GetAll(FieldAddress address)
        {
            if (data.ContainsKey(address))
                foreach (var e in data[address])
                    yield return e;

            foreach (var q in GetStarMatches(address))
                foreach (var e in starredData[q])
                    yield return e;
        }

       


        #region Сеты
        private void InternalSet(FieldAddress address, ADSpecification spec, object obj)
        {
            Dictionary<ADSpecification, object> ad;
            var cache = address.Elements.Contains("*") ? starredData : data;
            if (cache.ContainsKey(address))
                ad = cache[address];
            else
                cache[address] = ad = new Dictionary<ADSpecification, object>();
            if (obj != null)
                ad[spec] = obj;
            else
                ad.Remove(spec);
        }

        public void Set<Q>(FieldAddress address, ADSpecification<Q> spec, Q value)
        {
            InternalSet(address, spec, value);
        }

        public void Set(FieldAddress address, ADDefinition def)
        {
            InternalSet(address, def.ADSpec, def.ADValue);
        }
        #endregion
    }

}
