using Core.Application.Interfaces.Repository.Cache;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using NUnit.Framework;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Test {
    [TestFixture]
    public class RedisCacheTest {
        [TestCase]
        public void TestAddString() {
            //RedisCacheClient cl = new RedisCacheClient();
            //ICacheService _request = (ICacheService)new RedisCache();            
        }
    }
}
