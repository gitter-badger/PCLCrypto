﻿namespace PCLCrypto.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PCLTesting;

    [TestClass]
    public class KeyDerivationAlgorithmProviderTests
    {
        private readonly byte[] originalKey = new byte[] { 0x1, 0x2, 0x3, 0x5 };
        private readonly byte[] salt = new byte[8];
        private readonly int iterations = 100;
        private readonly Dictionary<KeyDerivationAlgorithm, string> stretchedKeyBase64 = new Dictionary<KeyDerivationAlgorithm, string> {
            { KeyDerivationAlgorithm.Pbkdf2Sha1, "3HWzwI225INl7y6+G9Jv7Af8UGE=" },
            { KeyDerivationAlgorithm.Pbkdf2Sha256, "t420R6yC8H2CDK/0sSGmwKHLooM=" },
        };

        [TestMethod]
        public void OpenAlgorithm()
        {
            var algorithm = WinRTCrypto.KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithm.Pbkdf2Sha1);
            Assert.IsNotNull(algorithm);
        }

        [TestMethod]
        public void Algorithm()
        {
            var algorithm = WinRTCrypto.KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithm.Pbkdf2Sha1);
            Assert.AreEqual(KeyDerivationAlgorithm.Pbkdf2Sha1, algorithm.Algorithm);

            algorithm = WinRTCrypto.KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithm.Pbkdf2Md5);
            Assert.AreEqual(KeyDerivationAlgorithm.Pbkdf2Md5, algorithm.Algorithm);
        }

        [TestMethod]
        public void CreateKey_InvalidInputs()
        {
            var algorithm = WinRTCrypto.KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithm.Pbkdf2Sha1);
            ExceptionAssert.Throws<ArgumentNullException>(
                () => algorithm.CreateKey(null));
        }

        [TestMethod]
        public void CreateKey()
        {
            foreach (var algorithmAndExpectedResult in this.stretchedKeyBase64)
            {
                Debug.WriteLine("Testing algorithm: {0}", algorithmAndExpectedResult.Key);
                var algorithm = WinRTCrypto.KeyDerivationAlgorithmProvider.OpenAlgorithm(algorithmAndExpectedResult.Key);
                ICryptographicKey key = algorithm.CreateKey(this.originalKey);
                Assert.IsNotNull(key);
                Assert.AreEqual(this.originalKey.Length * 8, key.KeySize);

                IKeyDerivationParameters parameters = WinRTCrypto.KeyDerivationParameters.BuildForPbkdf2(this.salt, this.iterations);
                Assert.AreEqual(this.iterations, parameters.IterationCount);
                CollectionAssertEx.AreEqual(this.salt, parameters.KdfGenericBinary);

                try
                {
                    byte[] keyMaterial = WinRTCrypto.CryptographicEngine.DeriveKeyMaterial(key, parameters, 20);
                    Assert.AreEqual(algorithmAndExpectedResult.Value, Convert.ToBase64String(keyMaterial));
                }
                catch (NotSupportedException)
                {
                    Debug.WriteLine(" - Not supported on this platform");
                }
            }
        }
    }
}
