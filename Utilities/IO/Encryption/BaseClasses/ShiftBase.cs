﻿/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using Utilities.IO.Encryption.Interfaces;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO.Encryption.BaseClasses
{
    /// <summary>
    /// Shift based encryption base class
    /// </summary>
    public abstract class ShiftBase:IShift
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ShiftBase()
        {
        }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }


        /// <summary>
        /// Encrypts the data based on the key
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The encrypted data</returns>
        public byte[] Encrypt(byte[] Data, byte[] Key)
        {
            Contract.Requires<ArgumentNullException>(Key != null, "Key");
            if (Data == null)
                return null;
            return Process(Data, Key);
        }

        /// <summary>
        /// Actually does the encryption/decryption
        /// </summary>
        private static byte[] Process(byte[] Input, byte[] Key)
        {
            byte[] OutputArray = new byte[Input.Length];
            int Position = 0;
            for (int x = 0; x < Input.Length; ++x)
            {
                OutputArray[x] = (byte)(Input[x] ^ Key[Position]);
                ++Position;
                if (Position >= Key.Length)
                    Position = 0;
            }
            return OutputArray;
        }

        /// <summary>
        /// Decrypt the data based on the key
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The decrypted data</returns>
        public byte[] Decrypt(byte[] Data, byte[] Key)
        {
            Contract.Requires<ArgumentNullException>(Key != null, "Key");
            if (Data == null)
                return null;
            return Process(Data, Key);
        }
    }
}