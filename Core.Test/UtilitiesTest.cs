using Core.Application.DTOs.Local;
using Core.Shared;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Test {
    [TestFixture]
    public class UtilitiesTest {
        private string dictionary;
        public UtilitiesTest() {
            dictionary = "vKeojwc8lfas0B1HhXbzEqGD9CxMOy5nm2Atr7pVPuULS3iQYNkRW4TJIFgZd6";
        }
        [TestCase]
        public void testAllPatternUtilitiesFeatures() {
            bool isAlphanumeric = Utilities.isAlphaNumeric("9809JARepoM");
            bool notAlphanumeric = Utilities.isAlphaNumeric("90000000");
            bool isAnMd5 = Utilities.isMD5("5ac749fbeec93607fc28d666be85e73a");
            bool isNotMd5 = Utilities.isMD5("5ac749fbeec93607666be85e73a");
            bool isHexStr = Utilities.isHexStr("5ac749fbeec93607fc28d666be85e73a");
            bool isNotHexStr = Utilities.isHexStr("5ac749fbeec93607fc28dy666be85e73a");
            bool isPasswordCase = Utilities.passwordCase("P@ssw0rd");
            bool isNotPasswordCase = Utilities.passwordCase("Passw0rd");
            bool isPhone = Utilities.isPhone("08167565432");
            bool isPhone2 = Utilities.isPhone("2348167565432");
            bool isNotPhone = Utilities.isPhone("55555555555");
            bool isNotPhone2 = Utilities.isPhone("04187654329");
            Assert.IsTrue(isAlphanumeric, "Should match a valid alphanumeric, regardless of case");
            Assert.IsFalse(notAlphanumeric, "Should not match a non AN");
            Assert.IsTrue(isAnMd5, "Should match a valid MD5");
            Assert.IsFalse(isNotMd5, "Should not match an invalid/curruped MD5");
            Assert.IsTrue(isHexStr, "Should match a valid Hex string");
            Assert.IsFalse(isNotHexStr, "Should not match an invalid/currupted Hex str");
            Assert.IsTrue(isPasswordCase, "Should match a valid password. At least 1 each of Upper case, special chars, number");
            Assert.IsFalse(isNotPasswordCase, "Should not match a password which does not satisfy any of the above");
            Assert.IsTrue(isPhone, "Should match a valid local phone without extension");
            Assert.IsTrue(isPhone2, "Should match a valid local phone with extension");
            Assert.IsFalse(isNotPhone, "Should not match an invalid extension");
            Assert.IsFalse(isNotPhone2, "Should not match an invalid phone without extension");
        }

        [TestCase]
        public void testDateUtilitiesFeatures() {
            long getTimeStamp = Utilities.getTimeStamp(1970, 1, 1, 0, 0, 0, 0);
            (DateTime modernDate, double unixTimestamp, double unixTimestampMS) = Utilities.getTodayDate();
            int ms = (int)(unixTimestampMS - unixTimestamp);
            DateTime date = Utilities.unixTimeStampToDateTime(unixTimestamp);
            Assert.IsTrue(ms < 1000, "Difference between UnixSeconds and UnixMillisecs should be less than 1000");
            Assert.AreEqual(date, modernDate, "Reversal of Unix to dateTime should be consistent");
            Assert.IsTrue(getTimeStamp == 0, "Unix at said day should be 0 seconds old");
        }

        [TestCase]
        public void testJObjectHelper() {
            List<object> children = new List<object> { new { roleName = "Supervisor", status = 0 }, new { roleName = "Staff", status = 1 } };
            object child = new { job = "Engeneer", salary = 200000 };
            object randomObj = new { name = "Some Name", age = 87, isAdmin = false, Roles = children, JD = child };
            JObject jObjectRandomObj = Utilities.asJObject(randomObj);
            string name = Utilities.findString(jObjectRandomObj, "name");
            string notFoundAttr = Utilities.findString(jObjectRandomObj, "name0");
            double? age = Utilities.findNumber(jObjectRandomObj, "age");
            JArray roles = Utilities.findArray(jObjectRandomObj, "Roles");
            JObject jd = Utilities.findObj(jObjectRandomObj, "JD");
            Assert.IsTrue(jObjectRandomObj != null, "Jobject conversion Succeded");
            Assert.IsTrue(name == "Some Name", "findstring should return a valid value");
            Assert.IsTrue(notFoundAttr == null, "findstring should return null for invalid idex/key");
            Assert.IsTrue(age == 87, "Find number should return number as in original object indexed");
            Assert.IsTrue(roles != null, "findArray should return collection as JArray");
            Assert.IsTrue(roles.Count == 2, "findArray should return exactly same count as in original object");
            Assert.IsTrue(jd != null, "findObj should return object as JObject");
            Assert.IsTrue(jd["job"].ToString() == "Engeneer", "findObj should return correctly index value");
        }
    }
}
