using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace WL3.CharacterMigrator
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SaveData
    {
        /**
         * Properties
         */

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<string> Header { get; }

        /// <summary>
        /// 
        /// </summary>
        public XDocument SaveState { get; }

        /// <summary>
        /// 
        /// </summary>
        public XElement Root 
        {
            get 
            {
                if (SaveState != null)
                    return SaveState.Root;
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public XElement this[string element]
        {
            get
            {
                if (SaveState != null)
                    if (SaveState.Root != null)
                        return SaveState.Root.Element(element);

                return null;
            }
        }

        /**
         * Methods
         */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="header"></param>
        /// <param name="saveState"></param>
        private SaveData(IEnumerable<string> header, XDocument saveState)
        {
            this.Header = header.ToList().AsReadOnly();
            this.SaveState = saveState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SaveData Load(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            return Load(fileBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SaveData Load(Stream stream)
        {
            byte[] fileBytes = new byte[stream.Length];
            if (stream.Read(fileBytes, 0, (int)stream.Length) == stream.Length)
                return Load(fileBytes);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        private static SaveData Load(byte[] fileBytes)
        {
            int dataOffset = 0;

            List<string> headerData = new List<string>();
            if (string.Equals("XLZF", Encoding.ASCII.GetString(fileBytes, 0, 4), StringComparison.OrdinalIgnoreCase))
            {
                for (int index = 0; index < 11; ++index)
                {
                    int headerOffset = Array.FindIndex(fileBytes, dataOffset, b => b == (byte)10) + 1;

                    headerData.Add(Encoding.UTF8.GetString(fileBytes, dataOffset, headerOffset - dataOffset));
                    dataOffset = headerOffset;
                }
            }

            byte[] inputBytes = new byte[fileBytes.Length - dataOffset];
            Array.Copy(fileBytes, dataOffset, inputBytes, 0, fileBytes.Length - dataOffset);

            XDocument saveState = XDocument.Load(new MemoryStream(CLZF2.Decompress(inputBytes)));

            return new SaveData(headerData, saveState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        public int Save(string path, out int dataSize)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = false
            };

            using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
                this.SaveState.Save(writer);

            byte[] saveData = memoryStream.ToArray();
            byte[] buffer = CLZF2.Compress(saveData);
            using (FileStream fileStream = File.Create(path))
            {
                for (int index = 0; index < this.Header.Count; ++index)
                {
                    string str = this.Header[index];
                    if (index == 4 || index == 5)
                        str = Regex.Replace(str, "(\\d+)", index == 4 ? saveData.Length.ToString() : buffer.Length.ToString());
                    
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    fileStream.Write(bytes, 0, bytes.Length);
                }

                fileStream.Write(buffer, 0, buffer.Length);

                dataSize = saveData.Length;
                return buffer.Length;
            }
        }
    } // public sealed class SaveData
} // namespace WL3.CharacterMigrator
