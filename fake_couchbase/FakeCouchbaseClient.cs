using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase;
using Couchbase.Operations;
using Couchbase.Results;
using Enyim;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;

namespace fake_couchbase
{
    public class FakeCouchbaseClient:ICouchbaseClient
    {
        private readonly CouchbaseServer _server;

        public FakeCouchbaseClient():this(new CouchbaseServer())
        {
            
        }
        public FakeCouchbaseClient(CouchbaseServer server)
        {
            _server = server;
        }

        public void Dispose()
        {
            
        }

        public object Get(string key)
        {
            var value = _server.GetItem(key);

            return value;
        }

        public T Get<T>(string key)
        {
            var value = (T) _server.GetItem(key);

            return value;
        }

        public IDictionary<string, object> Get(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string key, out object value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetWithCas(string key, out CasResult<object> value)
        {
            throw new NotImplementedException();
        }

        public CasResult<object> GetWithCas(string key)
        {
            throw new NotImplementedException();
        }

        public CasResult<T> GetWithCas<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, CasResult<object>> GetWithCas(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public bool Append(string key, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public CasResult<bool> Append(string key, ulong cas, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public bool Prepend(string key, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public CasResult<bool> Prepend(string key, ulong cas, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public bool Store(StoreMode mode, string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Store(StoreMode mode, string key, object value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public bool Store(StoreMode mode, string key, object value, TimeSpan validFor)
        {
            throw new NotImplementedException();
        }

        public CasResult<bool> Cas(StoreMode mode, string key, object value)
        {
            throw new NotImplementedException();
        }

        public CasResult<bool> Cas(StoreMode mode, string key, object value, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<bool> Cas(StoreMode mode, string key, object value, DateTime expiresAt, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<bool> Cas(StoreMode mode, string key, object value, TimeSpan validFor, ulong cas)
        {
            throw new NotImplementedException();
        }

        public ulong Decrement(string key, ulong defaultValue, ulong delta)
        {
            throw new NotImplementedException();
        }

        public ulong Decrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public ulong Decrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor)
        {
            throw new NotImplementedException();
        }

        public CasResult<ulong> Decrement(string key, ulong defaultValue, ulong delta, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<ulong> Decrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<ulong> Decrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor, ulong cas)
        {
            throw new NotImplementedException();
        }

        public ulong Increment(string key, ulong defaultValue, ulong delta)
        {
            throw new NotImplementedException();
        }

        public ulong Increment(string key, ulong defaultValue, ulong delta, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public ulong Increment(string key, ulong defaultValue, ulong delta, TimeSpan validFor)
        {
            throw new NotImplementedException();
        }

        public CasResult<ulong> Increment(string key, ulong defaultValue, ulong delta, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<ulong> Increment(string key, ulong defaultValue, ulong delta, DateTime expiresAt, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<ulong> Increment(string key, ulong defaultValue, ulong delta, TimeSpan validFor, ulong cas)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key, ulong cas)
        {
            throw new NotImplementedException();
        }

        public void FlushAll()
        {
            throw new NotImplementedException();
        }

        public ServerStats Stats()
        {
            throw new NotImplementedException();
        }

        public ServerStats Stats(string type)
        {
            throw new NotImplementedException();
        }

        public Guid Identity
        {
            get { throw new NotImplementedException(); }
        }

        public event Action<IMemcachedNode> NodeFailed;
        public IGetOperationResult ExecuteGet(string key)
        {
            var result =  ExecuteGet<object>(key);

            return new GetOperationResult
            {
                Cas = result.Cas,
                Exception = result.Exception,
                InnerResult = result.InnerResult,
                Message = result.Message,
                StatusCode = result.StatusCode,
                Success = result.Success,
                Value = result.Value
            };
        }

        public IGetOperationResult<T> ExecuteGet<T>(string key)
        {
            if (!_server.ItemExists(key))
            {
                return new GetOperationResult<T>
                {
                    Success = false,
                    StatusCode = (int) StatusCode.KeyNotFound
                };
            }

            var value = (T) _server.GetItem(key);
            return new GetOperationResult<T>
            {
                Success = true,
                Value = value,
                StatusCode = (int) StatusCode.Success
            };
        }

        public IDictionary<string, IGetOperationResult> ExecuteGet(IEnumerable<string> keys)
        {
            var results = keys
                .Select(key=>new{key,value=ExecuteGet(key)})
                .ToDictionary(key => key.key,value=>value.value);

            return results;
        }

        public IGetOperationResult ExecuteTryGet(string key, out object value)
        {
            throw new NotImplementedException();
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value)
        {
            switch (mode)
            {
                case StoreMode.Add:
                {
                    if (_server.ItemExists(key))
                        return new StoreOperationResult {Success = false, StatusCode = (int) StatusCode.KeyExists};
                    
                    _server.AddItem(key,value);
                    return new StoreOperationResult {StatusCode = (int) StatusCode.Success, Success = true};
                }
                case StoreMode.Set:
                {
                    _server.UpdateItem(key, value);
                    return new StoreOperationResult { StatusCode = (int) StatusCode.Success, Success = true };
                }
                case StoreMode.Replace:
                {
                    if (!_server.ItemExists(key))
                        return new StoreOperationResult { Success = false, StatusCode = (int) StatusCode.KeyNotFound};

                    _server.UpdateItem(key, value);
                    return new StoreOperationResult { StatusCode = (int) StatusCode.Success, Success = true };
                }
            }

            return new StoreOperationResult();
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, DateTime expiresAt)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, TimeSpan validFor)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteCas(StoreMode mode, string key, object value)
        {
            throw new NotImplementedException();
        }

        public IStoreOperationResult ExecuteCas(StoreMode mode, string key, object value, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IStoreOperationResult ExecuteCas(StoreMode mode, string key, object value, DateTime expiresAt, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IStoreOperationResult ExecuteCas(StoreMode mode, string key, object value, TimeSpan validFor, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteDecrement(string key, ulong defaultValue, ulong delta)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteDecrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteDecrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteDecrement(string key, ulong defaultValue, ulong delta, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteDecrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteDecrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteIncrement(string key, ulong defaultValue, ulong delta)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteIncrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteIncrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteIncrement(string key, ulong defaultValue, ulong delta, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteIncrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IMutateOperationResult ExecuteIncrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IConcatOperationResult ExecuteAppend(string key, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public IConcatOperationResult ExecuteAppend(string key, ulong cas, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public IConcatOperationResult ExecutePrepend(string key, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public IConcatOperationResult ExecutePrepend(string key, ulong cas, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public IRemoveOperationResult ExecuteRemove(string key)
        {
            throw new NotImplementedException();
        }

        public IRemoveOperationResult ExecuteRemove(string key, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult ExecuteGet(string key, DateTime newExpiration)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult<T> ExecuteGet<T>(string key, DateTime newExpiration)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult ExecuteTryGet(string key, DateTime newExpiration, out object value)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult ExecuteGetWithLock(string key)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult<T> ExecuteGetWithLock<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult ExecuteGetWithLock(string key, TimeSpan lockExpiration)
        {
            throw new NotImplementedException();
        }

        public IGetOperationResult<T> ExecuteGetWithLock<T>(string key, TimeSpan lockExpiration)
        {
            throw new NotImplementedException();
        }

        public IUnlockOperationResult ExecuteUnlock(string key, ulong cas)
        {
            throw new NotImplementedException();
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, PersistTo persistTo,
            ReplicateTo replicateTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, ReplicateTo replicateTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, PersistTo persistTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, DateTime expiresAt, PersistTo persistTo,
            ReplicateTo replicateTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, DateTime expiresAt, PersistTo persistTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, DateTime expiresAt, ReplicateTo replicateTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, TimeSpan validFor, PersistTo persistTo,
            ReplicateTo replicateTo)
        {

            return ExecuteStore(mode, key, value);
        }

        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, TimeSpan validFor, PersistTo persistTo)
        {
            return ExecuteStore(mode, key, value);
        }
        
        public IStoreOperationResult ExecuteStore(StoreMode mode, string key, object value, TimeSpan validFor, ReplicateTo replicateTo)
        {
            return ExecuteStore(mode, key, value);
        }

        public IRemoveOperationResult ExecuteRemove(string key, PersistTo persisTo, ReplicateTo replicateTo)
        {
            throw new NotImplementedException();
        }

        public IRemoveOperationResult ExecuteRemove(string key, PersistTo persisTo)
        {
            throw new NotImplementedException();
        }

        public IRemoveOperationResult ExecuteRemove(string key, ReplicateTo replicateTo)
        {
            throw new NotImplementedException();
        }

        public IObserveOperationResult Observe(string key, ulong cas, PersistTo persistTo, ReplicateTo replicateTo,
            ObserveKeyState persistedKeyState = ObserveKeyState.FoundPersisted, ObserveKeyState replicatedState = ObserveKeyState.FoundNotPersisted)
        {
            throw new NotImplementedException();
        }

        public object Get(string key, DateTime newExpiration)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, DateTime newExpiration)
        {
            throw new NotImplementedException();
        }

        public CasResult<object> GetWithLock(string key)
        {
            throw new NotImplementedException();
        }

        public CasResult<T> GetWithLock<T>(string key)
        {
            throw new NotImplementedException();
        }

        public CasResult<object> GetWithLock(string key, TimeSpan lockExpiration)
        {
            throw new NotImplementedException();
        }

        public CasResult<T> GetWithLock<T>(string key, TimeSpan lockExpiration)
        {
            throw new NotImplementedException();
        }

        public bool Unlock(string key, ulong cas)
        {
            throw new NotImplementedException();
        }

        public CasResult<object> GetWithCas(string key, DateTime newExpiration)
        {
            throw new NotImplementedException();
        }

        public CasResult<T> GetWithCas<T>(string key, DateTime newExpiration)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, SyncResult> Sync(SyncMode mode, IEnumerable<KeyValuePair<string, ulong>> items)
        {
            throw new NotImplementedException();
        }

        public SyncResult Sync(string key, ulong cas, SyncMode mode)
        {
            throw new NotImplementedException();
        }

        public SyncResult Sync(string key, ulong cas, SyncMode mode, int replicationCount)
        {
            throw new NotImplementedException();
        }

        public void Touch(string key, DateTime nextExpiration)
        {
            throw new NotImplementedException();
        }

        public void Touch(string key, TimeSpan nextExpiration)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string key, DateTime newExpiration, out object value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetWithCas(string key, DateTime newExpiration, out CasResult<object> value)
        {
            throw new NotImplementedException();
        }

        public IView<IViewRow> GetView(string designName, string viewName)
        {
            throw new NotImplementedException();
        }

        public IView<IViewRow> GetView(string designName, string viewName, bool urlEncode)
        {
            throw new NotImplementedException();
        }

        public IView<T> GetView<T>(string designName, string viewName, bool shouldLookupDocById = false)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> Get(IView view)
        {
            throw new NotImplementedException();
        }

        public ISpatialView<ISpatialViewRow> GetSpatialView(string designName, string viewName)
        {
            throw new NotImplementedException();
        }

        public ISpatialView<T> GetSpatialView<T>(string designName, string viewName, bool shouldLookupDocById = false)
        {
            throw new NotImplementedException();
        }

        public bool KeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public bool KeyExists(string key, ulong cas)
        {
            throw new NotImplementedException();
        }
    }
}