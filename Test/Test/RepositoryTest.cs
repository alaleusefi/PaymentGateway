using System;
using Xunit;
using Core;
using Moq;

namespace Test
{
    public class RepositoryTest
    {
        private readonly FakeRepo<object> instance = new FakeRepo<object>();

        [Fact]
        public void Save_AssignsId_Sequentially()
        {
            var item0 = new object();
            var id0 = instance.Save(item0);

            var item1 = new object();
            var id1 = instance.Save(item1);

            Assert.Equal(id1, id0 + 1);
        }

        [Fact]
        public void Get_ReturnsNull_IfNotFound()
        {
            var item = instance.Get(7);
            Assert.Null(item);
        }

        [Fact]
        public void Get_RetrievesItem()
        {
            var item = new object();
            var id = instance.Save(item);
            var retrieved = instance.Get(id);
            Assert.Same(item, retrieved);
        }
    }
}
