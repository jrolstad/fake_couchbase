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
    }
}