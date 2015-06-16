using System;
using Couchbase.Extensions;
using Enyim;
using Enyim.Caching.Memcached;
using NUnit.Framework;

namespace fake_couchbase.tests
{
    [TestFixture]
    public class FakeCouchbaseClientTests
    {
        [Test]
        public void ExecuteStore_Adding_ReturnsSuccess()
        {
            // Arrange
            var client = new FakeCouchbaseClient();
            
            // Act
            var result = client.ExecuteStore(StoreMode.Add, "my_key", "some_value");

            // Assert
            Assert.That(result.Success,Is.True);
            Assert.That(result.StatusCode,Is.EqualTo((int) StatusCode.Success));
        }

        [Test]
        public void ExecuteStore_AddingExistingItem_ReturnsFailure()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            client.ExecuteStore(StoreMode.Add, "my_key", "some_value");

            // Act
            var result = client.ExecuteStore(StoreMode.Add, "my_key", "some other value");

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.KeyExists));
        }

        [Test]
        public void ExecuteStore_Set_ReturnsSuccess()
        {
            // Arrange
            var client = new FakeCouchbaseClient();
            
            // Act
            var result = client.ExecuteStore(StoreMode.Set, "my_key", "some_value");

            // Assert
            Assert.That(result.Success,Is.True);
            Assert.That(result.StatusCode,Is.EqualTo((int) StatusCode.Success));
        }

        [Test]
        public void ExecuteStore_SetExistingItem_ReturnsSuccess()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            client.ExecuteStore(StoreMode.Add, "my_key", "some_value");

            // Act
            var result = client.ExecuteStore(StoreMode.Set, "my_key", "some other value");

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.Success));
        }

        [Test]
        public void ExecuteStore_ReplaceExisting_ReturnsSuccess()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            client.ExecuteStore(StoreMode.Add, "my_key", "some_value");

            // Act
            var result = client.ExecuteStore(StoreMode.Replace, "my_key", "some other value");

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.Success));
        }

        [Test]
        public void ExecuteStore_ReplaceNoExisting_ReturnsFailure()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            // Act
            var result = client.ExecuteStore(StoreMode.Replace, "my_key", "some_value");

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.KeyNotFound));
        }

        [Test]
        public void ExecuteStoreJson_Add_StoresTheValue()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            var instance = new MyTestType {Id1 = Guid.NewGuid(), Id2 = null};

            // Act
            var result = client.ExecuteStoreJson(StoreMode.Add, instance.Id1.ToString(), instance);

            // Assert
            Assert.That(result.Success,Is.True);
            Assert.That(result.StatusCode,Is.EqualTo((int) StatusCode.Success));

            var value = client.GetJson<MyTestType>(instance.Id1.ToString());
            Assert.That(value,Is.EqualTo(instance));
        }

        [Test]
        public void ExecuteStoreJson_AddExistingId_Fails()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            var instance = new MyTestType { Id1 = Guid.NewGuid(), Id2 = null };
            var firstInstance = new MyTestType { Id1 = Guid.NewGuid(), Id2 = null };

            client.ExecuteStoreJson(StoreMode.Add, "my_key", firstInstance);

            // Act
            var result = client.ExecuteStoreJson(StoreMode.Add, "my_key", instance);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.KeyExists));

            var value = client.GetJson<MyTestType>("my_key");
            Assert.That(value, Is.EqualTo(firstInstance));


        }

        [Test]
        public void ExecuteGetJson_ExistingItem_GetsTheValue()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            var instance = new MyTestType { Id1 = Guid.NewGuid(), Id2 = null };
            client.ExecuteStoreJson(StoreMode.Add, "my_key", instance);

            // Act
            var result = client.ExecuteGetJson<MyTestType>("my_key");

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.Success));
            Assert.That(result.Value, Is.EqualTo(instance));
        }

        [Test]
        public void ExecuteGetJson_NonExistentItem_Fails()
        {
            // Arrange
            var client = new FakeCouchbaseClient();


            // Act
            var result = client.ExecuteGetJson<MyTestType>("my_key");

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.KeyNotFound));
        }

        [Test]
        public void Get_Keys_ReturnsMatchingKeys()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            client.ExecuteStore(StoreMode.Add, "key1", "my_value1");
            client.ExecuteStore(StoreMode.Add, "key2", "my_value2");
            client.ExecuteStore(StoreMode.Add, "key3", "my_value3");

            // Act
            var result = client.Get(new[] {"key1", "key3"});

            // Assert
            Assert.That(result.Count,Is.EqualTo(2));
            Assert.That(result["key1"],Is.EqualTo("my_value1"));
            Assert.That(result["key3"],Is.EqualTo("my_value3"));
        }

        [Test]
        public void Get_ObjectNonExistent_ReturnsDefaultValue()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            // Act
            var result = client.Get("some_key");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_StringNonExistent_ReturnsDefaultValue()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            // Act
            var result = client.Get<string>("some_key");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_Object_ReturnsValue()
        {
            // Arrange
            var client = new FakeCouchbaseClient();
            client.Store(StoreMode.Add, "some_key", "my value");

            // Act
            var result = client.Get("some_key");

            // Assert
            Assert.That(result, Is.EqualTo("my value"));
        }

        [Test]
        public void Get_String_ReturnsValue()
        {
            // Arrange
            var client = new FakeCouchbaseClient();
            client.Store(StoreMode.Add, "some_key", "my value");

            // Act
            var result = client.Get<string>("some_key");

            // Assert
            Assert.That(result, Is.EqualTo("my value"));
        }

        [Test]
        public void ExecuteRemove_ExistingItem_ReturnsSuccess()
        {
            // Arrange
            var client = new FakeCouchbaseClient();
            client.Store(StoreMode.Add, "my_key", "some value");

            // Act
            var result = client.ExecuteRemove("my_key");

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.Success));
        }

        [Test]
        public void ExecuteRemove_NonExistingItem_ReturnsKeyNotFoundFailure()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            // Act
            var result = client.ExecuteRemove("my_key");

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo((int)StatusCode.KeyNotFound));
        }

        [Test]
        public void Remove_ExistingItem_ReturnsSuccess()
        {
            // Arrange
            var client = new FakeCouchbaseClient();
            client.Store(StoreMode.Add, "my_key", "some value");

            // Act
            var result = client.Remove("my_key");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Remove_NonExistingItem_ReturnsKeyNotFoundFailure()
        {
            // Arrange
            var client = new FakeCouchbaseClient();

            // Act
            var result = client.Remove("my_key");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Touch_ExistingItem_UpdatesExpiration()
        {
            // Arrange
            var server = new CouchbaseServer();
            var client = new FakeCouchbaseClient(server);
            client.Store(StoreMode.Add, "my_key", "some value");

            var expiresAt = DateTime.Now.AddMinutes(3);

            // Act
            client.Touch("my_key", expiresAt);

            // Assert
            Assert.That(server.GetItem("my_key").Expiration, Is.EqualTo(expiresAt));
        }

        [Test]
        public void Touch_NonExistingItem_DoesNothing()
        {
            // Arrange
            var server = new CouchbaseServer();
            var client = new FakeCouchbaseClient(server);

            var expiresAt = DateTime.Now.AddMinutes(3);

            // Act
            client.Touch("my_key", expiresAt);

            // Assert
            Assert.That(server.ItemExists("my_key"), Is.False);
        }
    }
}