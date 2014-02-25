﻿//-----------------------------------------------------------------------
// <copyright file="ICryptographicKey.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PCLCrypto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A cryptographic key.
    /// </summary>
    public interface ICryptographicKey
    {
        /// <summary>
        /// Gets the size, in bits, of the key.
        /// </summary>
        /// <value>
        /// Integer that contains the number of bits in the key.
        /// </value>
        int KeySize { get; }

        /// <summary>
        /// Exports the key pair to a buffer.
        /// </summary>
        /// <returns>Buffer that contains the key pair.</returns>
        byte[] Export();

        /// <summary>
        /// Exports the key pair to a buffer given a specified format.
        /// </summary>
        /// <param name="blobType">A CryptographicPrivateKeyBlobType enumeration value that specifies the format
        /// of the key in the buffer. The default value is Pkcs8RawPrivateKeyInfo.</param>
        /// <returns>
        /// Buffer that contains the key pair.
        /// </returns>
        byte[] Export(CryptographicPrivateKeyBlobType blobType);

        /// <summary>
        /// Exports a public key to a buffer.
        /// </summary>
        /// <returns>
        /// Buffer that contains the public key.
        /// </returns>
        byte[] ExportPublicKey();

        /// <summary>
        /// Exports a public key to a buffer given a specified format.
        /// </summary>
        /// <param name="blobType">A CryptographicPublicKeyBlobType enumeration value that specifies the format
        /// of the key in the buffer. The default value is X509SubjectPublicKeyInfo.</param>
        /// <returns>
        /// Buffer that contains the public key.
        /// </returns>
        byte[] ExportPublicKey(CryptographicPublicKeyBlobType blobType);
    }
}