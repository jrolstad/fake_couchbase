using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace fake_couchbase
{
    public class CouchbaseServer
    {
        public ConcurrentDictionary<string,object> BucketValues = new ConcurrentDictionary<string, object>();

        public bool ItemExists(string key)
        {
            return BucketValues.ContainsKey(key);
        }

        public void AddItem(string key, object value)
        {
            BucketValues.AddOrUpdate(key, s => value, (s, o) => value);
        }

        public void UpdateItem(string key, object value)
        {
            BucketValues.AddOrUpdate(key, s => value, (s, o) => value);
        }

        public void DeleteItem(string key)
        {
            object value;
            BucketValues.TryRemove(key, out value);
        }

        public object GetItem(string key)
        {
            return BucketValues[key];
        }

        public IDictionary<string,object> GetItem(IEnumerable<string> keys)
        {
            var values = BucketValues
                .ToArray()
                .Where(item => keys.Contains(item.Key))
                .ToDictionary(item => item.Key, item => item.Value);

            return values;
        }

        public void RemoveItem(string key)
        {
            object value;
            BucketValues.TryRemove(key, out value);
        }
    }

    
}