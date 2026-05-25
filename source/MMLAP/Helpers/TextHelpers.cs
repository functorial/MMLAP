using Archipelago.Core.Util;
using MMLAP.Models;
using static MMLAP.Models.MMLEnums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMLAP.Helpers
{
    public class TextHelpers
    {
        public static readonly byte lsqbracket = 0x0F;
        public static readonly byte dot = 0x10;
        public static readonly byte yen = 0x4D;
        public static readonly byte rsqbracket = 0x50;
        public static readonly byte circle = 0x54;
        public static readonly byte triangle = 0x55;
        public static readonly byte cross = 0x56;
        public static readonly byte square = 0x57;
        public static readonly byte L1 = 0x58;
        public static readonly byte L2 = 0x59;
        public static readonly byte R1 = 0x5A;
        public static readonly byte R2 = 0x5B;
        public static readonly byte ellipses = 0x5F;
        public static readonly byte pointer = 0x60;
        public static readonly byte alpha = 0x80;
        public static readonly byte omega = 0x81;
        public static readonly byte textColorWhite = 0x00;
        public static readonly byte textColorRed = 0x02;
        public static readonly byte textColorGreen = 0x03;
        public static readonly byte textColorBlue = 0x04;
        public static readonly byte textColorPurple = 0x05;
        public static readonly byte textColorCool1 = 0x58;
        public static readonly byte textColorCool2 = 0xB0;
        public static readonly byte[] youGot = [0x2E, 0x3F, 0x45, 0x4F, 0x36, 0x3F, 0x44, 0x13, 0x86];
        public static readonly byte[] youGotSound = [0x8E, 0x86, 0x00];
        public static readonly byte[] redAPItem = [0x89, 0x02, 0x15, 0x24, 0x4F, 0x1D, 0x44, 0x34, 0x3C, 0x89, 0x00];
        public static readonly byte[] nothing = [0x22, 0x3F, 0x44, 0x37, 0x38, 0x3D, 0x36];
        public static readonly byte[] newPage = [0x9F, 0x87, 0x04, 0x00];
        public static readonly byte[] endWindow = [0x9F, 0xA9, 0x84, 0x04, 0x00];

        public static readonly Dictionary<char, byte> charDict = new()
        {
            { '0', 0x00 },
            { '1', 0x01 },
            { '2', 0x02 },
            { '3', 0x03 },
            { '4', 0x04 },
            { '5', 0x05 },
            { '6', 0x06 },
            { '7', 0x07 },
            { '8', 0x08 },
            { '9', 0x09 },
            { '\'', 0x0C },
            { '!', 0x0D },
            { '?', 0x0E },
            { '(', 0x11 },
            { ')', 0x12 },
            { ':', 0x13 },
            { 'A', 0x15 },
            { 'B', 0x16 },
            { 'C', 0x17 },
            { 'D', 0x18 },
            { 'E', 0x19 },
            { 'F', 0x1A },
            { 'G', 0x1B },
            { 'H', 0x1C },
            { 'I', 0x1D },
            { 'J', 0x1E },
            { 'K', 0x1F },
            { 'L', 0x20 },
            { 'M', 0x21 },
            { 'N', 0x22 },
            { 'O', 0x23 },
            { 'P', 0x24 },
            { 'Q', 0x25 },
            { 'R', 0x26 },
            { 'S', 0x27 },
            { 'T', 0x28 },
            { 'U', 0x2A },
            { 'V', 0x2B },
            { 'W', 0x2C },
            { 'X', 0x2D },
            { 'Y', 0x2E },
            { 'Z', 0x2F },
            { 'a', 0x30 },
            { 'b', 0x31 },
            { 'c', 0x32 },
            { 'd', 0x33 },
            { 'e', 0x34 },
            { 'f', 0x35 },
            { 'g', 0x36 },
            { 'h', 0x37 },
            { 'i', 0x38 },
            { 'j', 0x39 },
            { 'k', 0x3A },
            { 'l', 0x3B },
            { 'm', 0x3C },
            { 'n', 0x3D },
            { 'o', 0x3F },
            { 'p', 0x40 },
            { 'q', 0x41 },
            { 'r', 0x42 },
            { 's', 0x43 },
            { 't', 0x44 },
            { 'u', 0x45 },
            { 'v', 0x46 },
            { 'w', 0x47 },
            { 'x', 0x48 },
            { 'y', 0x49 },
            { 'z', 0x4A },
            { '&', 0x4B },
            { '/', 0x4E },
            { ' ', 0x4F },
            { '~', 0x51 },
            { '-', 0x52 },
            { ',', 0x5C },
            { '"', 0x5D },
            { '.', 0x5E },
            { '+', 0x61 },
            { '%', 0x62 },
            { ';', 0x82 },
            { '=', 0x83 },
            { '\n', 0x86 },
           };

        public static string TranslateEncoding(byte[] encodedText)
        {
            Dictionary<byte, char> reversed = charDict.ToDictionary(x => x.Value, x => x.Key);
            string res = new(encodedText.Select(b => reversed.TryGetValue(b, out char c) ? c : '?').ToArray());
            return res;
        }

        public static byte[] ConcatArrays(byte[] x, byte[] y)
        {
            byte[] z = new byte[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);
            return z;
        }

        public static byte[] ConcatArrayList(List<byte[]> arrayList)
        {
            byte[] last = arrayList[0];
            byte[] res = last;
            for (int i = 0; i < (arrayList.Count - 1); i++)
            {
                byte[] next = arrayList[i+1];
                res = ConcatArrays(res, next);
            }
            return res;
        }

        public static byte[] EncodeSimpleString(string text)
        {
            char[] inArray = text.ToCharArray();
            byte[] outArray = new byte[inArray.Length];
            for (int i = 0; i < inArray.Length; i++)
            {
                if (charDict.TryGetValue(inArray[i], out byte value))
                {
                    outArray[i] = value;
                }
                else
                {
                    outArray[i] = 0x00; // handle missing char
                }
            }
            return outArray;
        }

        public static byte[] EncodeItemDisplay(byte itemCode)
        {
            byte[] itemDisplay = [0xD0, itemCode];
            return itemDisplay;
        }

        public static byte[] AddTextColor(byte[] encoding, byte textColor)
        {
            byte[] coloredEncoding = new byte[encoding.Length + 4];
            coloredEncoding[0] = 0x89;
            coloredEncoding[1] = textColor;
            encoding.CopyTo(coloredEncoding, 2);
            coloredEncoding[^2] = 0x89;
            coloredEncoding[^1] = 0x00; // reset to default color 
            return coloredEncoding;
        }

        public static byte[] PlaySound(byte soundCode)
        {
            // 0X81 to 0x84 = menu
            // 0x85 and 0x86 = "you got" sounds
            // 0x8A = stop all music
            // 0x8C to 0x9B = megaman
            // 0x9C = reaverbot sounds
            // 0x9D to 0x9E = enemy pickups
            // 0x9F = ledge grab
            // 0xA0 to 0xA4 = enemies getting hit
            // 0xD0 = can
            return [(byte)0x8E, soundCode, (byte)0x00];
        }

        public static byte[] EncodeYouGotItemWindow(ItemData itemData, byte[]? prefix = null, byte[]? suffix = null, uint? guaranteedLength = null)
        {
            prefix ??= [];
            suffix ??= endWindow;
            List<ItemCategory> displayedItemCategories =
            [
                ItemCategory.Buster,
                ItemCategory.Special,
                ItemCategory.Normal 
            ];
            byte[] itemByteArray = [];
            switch (itemData)
            {
                case var data when displayedItemCategories.Contains(data.Category):
                    itemByteArray = AddTextColor(EncodeItemDisplay(itemData.ItemCode ?? 0), textColorGreen);
                    break;
                case var data when data.Category == ItemCategory.Nothing:
                    itemByteArray = nothing; 
                    break;
                case var data when data.Category == ItemCategory.Zenny:
                    itemByteArray = EncodeSimpleString(itemData.Name);
                    break;
                case var data when data.Category == ItemCategory.AP:
                    itemByteArray = redAPItem;
                    break;
                case var data when data.NickName != null:
                    itemByteArray = AddTextColor(EncodeSimpleString(itemData.NickName ?? ""), textColorGreen);
                    break;
                default:
                    break;
            }
            byte[] spaceFill = [];
            if (guaranteedLength != null)
            {
                int nonItemLength = prefix.Length + youGotSound.Length + youGot.Length + 1 + suffix.Length;
                int maxItemLength = (int)guaranteedLength - nonItemLength;
                if (maxItemLength < 0)
                {
                    Serilog.Log.Logger.Warning("guaranteedLength ({GuaranteedLength}) is smaller than fixed non-item text length ({NonItemLength}) in EncodeYouGotItemWindow.", guaranteedLength, nonItemLength);
                    maxItemLength = 0;
                }

                if (itemByteArray.Length > maxItemLength)
                {
                    Serilog.Log.Logger.Warning("Truncating item text in EncodeYouGotItemWindow from {OriginalLength} bytes to {MaxLength} bytes to fit guaranteedLength ({GuaranteedLength}).", itemByteArray.Length, maxItemLength, guaranteedLength);
                    itemByteArray = itemByteArray.Take(maxItemLength).ToArray();
                }

                int totalLength = nonItemLength + itemByteArray.Length;
                spaceFill = Enumerable.Repeat((byte)0x4F, (int)guaranteedLength - totalLength).ToArray();
            }
            List<byte[]> substrs =
            [
                prefix,
                youGotSound,
                youGot,
                itemByteArray,
                [charDict['!']],
                spaceFill,
                suffix,
            ];
            return ConcatArrayList(substrs);
        }

        public static TextData OverwriteText(ulong startAddress, byte[] text)
        {
            ushort sourceLevelId = Memory.ReadUShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);
            TextData overwrittenTextData = new(startAddress, Memory.ReadByteArray(startAddress, text.Length), sourceLevelId);
            Memory.WriteByteArray(startAddress, text);
            return overwrittenTextData;
        }
    }
}
