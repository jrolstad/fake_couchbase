using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace fake_couchbase
{
    public class CouchbaseServer
    {
        public ConcurrentDictionary<string, BucketItem> BucketValues = new ConcurrentDictionary<string, BucketItem>();

        public bool ItemExists(string key)
        {
            return BucketValues.ContainsKey(key);
        }

        public void AddItem(string key, object value, DateTime? expiresAt)
        {
            BucketValues.AddOrUpdate(key, s => new BucketItem(value, expiresAt), (s, o) =>
            {
                o.Value = value;
                o.Expiration = expiresAt;
                return o;
            });
        }

        public void UpdateItem(string key, object value, DateTime? expiresAt)
        {
            BucketValues.AddOrUpdate(key, s => new BucketItem(value, expiresAt), (s, o) =>
            {
                o.Value = value;
                o.Expiration = expiresAt;
                return o;
            });
        }

        public BucketItem GetItem(string key)
        {
            return BucketValues[key];
        }

        public IDictionary<string,object> GetItem(IEnumerable<string> keys, DateTime currentDateTime)
        {
            var values = BucketValues
                .ToArray()
                .Where(item => keys.Contains(item.Key))
                .Where(item => item.Value.Expiration == null || item.Value.Expiration > currentDateTime)
                .ToDictionary(item => item.Key, item => item.Value.Value);

            return values;
        }

        public void RemoveItem(string key)
        {
            BucketItem value;
            BucketValues.TryRemove(key, out value);
        }


    }

    public class BucketItem
    {
        public BucketItem(object value, DateTime? expiration)
        {
            Value = value;
            Expiration = expiration;
        }
        public object Value { get; set; }

        public DateTime? Expiration { get; set; }
    }


    
}