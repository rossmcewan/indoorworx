// <copyright file="WVC1CodecPrivateDataParser.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: WVC1CodecPrivateDataParser.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using Infrastructure;
    using RCE.Services.Contracts;
    using SMPTETimecode;

    /// <summary>
    /// A Codec Private Data parser.
    /// </summary>
    public class WVC1CodecPrivateDataParser : ICodecPrivateDataParser
    {
        /// <summary>
        /// The term being searched on the manifest.
        /// </summary>
        private const string CodecPrivateDataTerm = @"CODECPRIVATEDATA=""";

        /// <summary>
        /// The logger used to log events.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WVC1CodecPrivateDataParser"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log events.</param>
        public WVC1CodecPrivateDataParser(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Occurs when the GetFrameRate operation is completed.
        /// </summary>
        public event EventHandler<DataEventArgs<SmpteFrameRate>> GetFrameRateCompleted;

        /// <summary>
        /// Parses a codec private data to get the frame rate.
        /// </summary>
        /// <param name="codecPrivateData">The codec private data to parse.</param>
        /// <returns>The frame rate.</returns>
        public SmpteFrameRate GetFrameRate(string codecPrivateData)
        {
            IDictionary<uint, int> numeratorMappings = new Dictionary<uint, int>
                                                                   {
                                                                       { 1, 24 * 1000 },
                                                                       { 2, 25 * 1000 },
                                                                       { 3, 30 * 1000 },
                                                                       { 4, 50 * 1000 },
                                                                       { 5, 60 * 1000 },
                                                                       { 6, 48 * 1000 },
                                                                       { 7, 72 * 1000 }
                                                                   };

            IDictionary<uint, int> denominatorMappings = new Dictionary<uint, int>
                                                                     {
                                                                         { 1, 1000 },
                                                                         { 2, 1001 }
                                                                     };
            const int SkipBitsInAllCases = 5 * 8;

            byte[] bytes = StringToByteArray(codecPrivateData);

            int frameRateStartsAt = SkipBitsInAllCases;

            if (GetBitValue(bytes, SkipBitsInAllCases + 46))
            {
                if (GetBitValue(bytes, SkipBitsInAllCases + 75))
                {
                    if (GetBitValue(bytes, SkipBitsInAllCases + 79)
                        && GetBitValue(bytes, SkipBitsInAllCases + 80)
                        && GetBitValue(bytes, SkipBitsInAllCases + 81)
                        && GetBitValue(bytes, SkipBitsInAllCases + 82))
                    {
                        frameRateStartsAt += 96;
                    }
                    else
                    {
                        frameRateStartsAt += 80;
                    }
                }
                else
                {
                    frameRateStartsAt += 76;
                }
            }
            else
            {
                return SmpteFrameRate.Unknown;
            }

            bool framerateFlag = GetBitValue(bytes, frameRateStartsAt);

            if (framerateFlag)
            {
                frameRateStartsAt++;

                bool framerateInd = GetBitValue(bytes, frameRateStartsAt);

                if (framerateInd)
                {
                    uint frameRateValue = GetBits(bytes, frameRateStartsAt, 16);

                    decimal value = (frameRateValue * 0.03125M) + 0.03125M;

                    return GetFrameRate(value);
                }
                else
                {
                    uint frameRateNumerator = GetBits(bytes, frameRateStartsAt, 8);

                    frameRateStartsAt += 8;

                    uint frameRateDenominator = GetBits(bytes, frameRateStartsAt, 4);

                    if (numeratorMappings.ContainsKey(frameRateNumerator) && denominatorMappings.ContainsKey(frameRateDenominator))
                    {
                        decimal numerator = numeratorMappings[frameRateNumerator];
                        decimal denominator = denominatorMappings[frameRateDenominator];

                        decimal value = numerator / denominator;

                        return GetFrameRate(value);
                    }
                    else
                    {
                        return SmpteFrameRate.Unknown;
                    }
                }
            }

            return SmpteFrameRate.Unknown;
        }

        /// <summary>
        /// Parses a manifest to get the frame rate associated with it.
        /// </summary>
        /// <param name="manifestUri">The manifest uri.</param>
        public void GetFrameRateAsync(Uri manifestUri)
        {
            Downloader downloader = new Downloader(this.logger);
            downloader.OpenReadCompleted += this.OnOpenReadCompleted;
            downloader.OpenReadAsync(manifestUri, null);
        }

        /// <summary>
        /// Gets the bit value of an specific position.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="p">The position to get the bit value.</param>
        /// <returns>The bit value.</returns>
        private static bool GetBitValue(byte[] bytes, int p)
        {
            int bytePos = p / 8;
            int bitPos = p % 8;

            return (bytes[bytePos] & (byte)(1 << (7 - bitPos))) != 0;
        }

        /// <summary>
        /// Converts a hex string into a byte array.
        /// </summary>
        /// <param name="hex">The hex string being converted.</param>
        /// <returns>The byte array.</returns>
        private static byte[] StringToByteArray(string hex)
        {
            int numberOfChars = hex.Length;
            byte[] bytes = new byte[numberOfChars / 2];
            
            for (int i = 0; i < numberOfChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Gets the bits value of an specific range.
        /// </summary>
        /// <param name="bytes">The bytes array.</param>
        /// <param name="offset">The start position.</param>
        /// <param name="count">The number of bytes to parse.</param>
        /// <returns>The bits value.</returns>
        private static uint GetBits(byte[] bytes, int offset, int count)
        {
            uint value = 0;
            for (int i = 1; i <= count; i++)
            {
                if (GetBitValue(bytes, offset + i))
                {
                    value += (uint) Math.Pow(2, count - i);
                }
            }

            return value;
        }

        /// <summary>
        /// Gets the frame rate.
        /// </summary>
        /// <param name="frameRate">The explicit frame rate value.</param>
        /// <returns>An <see cref="SmpteFrameRate"/> enum frameRate.</returns>
        private static SmpteFrameRate GetFrameRate(decimal frameRate)
        {
            if (((24M * 1000M) / 1001M) == frameRate)
            {
                return SmpteFrameRate.Smpte2398;
            }

            if (frameRate == 24)
            {
                return SmpteFrameRate.Smpte24;
            }

            if (frameRate == 25)
            {
                return SmpteFrameRate.Smpte25;
            }

            if (((30M * 1000M) / 1001M) == frameRate)
            {
                return SmpteFrameRate.Smpte2997NonDrop;
            }

            if (frameRate == 30)
            {
                return SmpteFrameRate.Smpte30;
            }

            return SmpteFrameRate.Unknown;
        }

        /// <summary>
        /// Reads the manifest and tries to extract the frame rate.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void OnOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                StreamReader reader = new StreamReader(e.Result);
                string manifest = reader.ReadToEnd().ToUpper(CultureInfo.InvariantCulture);
                reader.Close();

                int termIndex = manifest.IndexOf(CodecPrivateDataTerm);
                int termIndexLength = termIndex + CodecPrivateDataTerm.Length;

                int endTermIndex = manifest.IndexOf(@"""", termIndexLength);
                string codecPrivateData = manifest.Substring(termIndexLength, endTermIndex - termIndexLength);

                try
                {
                    SmpteFrameRate frameRate = this.GetFrameRate(codecPrivateData);
                    this.OnGetFrameRateCompleted(frameRate);
                }
                catch (Exception ex)
                {
                    this.logger.Log("WVC1CodecPrivateDataParser", ex);
                }
            }
        }

        /// <summary>
        /// Raises the GetFrameCompleted event.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        private void OnGetFrameRateCompleted(SmpteFrameRate frameRate)
        {
            EventHandler<DataEventArgs<SmpteFrameRate>> completed = this.GetFrameRateCompleted;
            if (completed != null)
            {
                completed(this, new DataEventArgs<SmpteFrameRate>(frameRate));
            }
        }
    }
}
