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
    }
}