using Core.Models.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Test {
    [TestFixture]
    public class BaseEntityTest {
        [TestCase]
        public void TestDefault() {
            BaseEntity baseE = new BaseEntity();
            Assert.IsTrue(baseE.isNullOrEmpty("", "n", null));
            Assert.IsFalse(baseE.isNullOrEmpty("h", "n"));
            Assert.IsFalse(baseE.isNull("h", ""));
            Assert.IsTrue(baseE.isNull("h", null));
        }
    }
}
