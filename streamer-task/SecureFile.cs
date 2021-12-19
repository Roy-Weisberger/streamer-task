using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace streamer_task
{
    public class SecureFile
    {
        private const int chunkSize = 1024;
        private const int hashSize = 32;

        // Encodes a file returns hash of the first block and status string 
        // returns true if succecful
        public bool FileEncoder(string file_in_path, string file_out_path, out string hash0,
            out string status)
        {
            FileStream streamIn = null;
            FileStream streamOut = null;
            try
            {
                streamIn = new FileStream(file_in_path, FileMode.Open, FileAccess.Read);
                streamOut = new FileStream(file_out_path, FileMode.Create);
                bool retValue = FileEncoder(streamIn, streamOut, out hash0, out status);
                streamIn.Close();
                streamOut.Close();
                return retValue;
            }
            catch (Exception ex)
            {
                hash0 = "";
                status = ex.Message;
                if (streamIn != null)
                    streamIn.Close();
                return false;
            }
        }

        // Encodes a file returns hash of the first block and status string 
        // returns true if succecful
        private bool FileEncoder(FileStream streamIn, FileStream streamOut, out string hash0,
                out string status)
        {
            SHA256 mySHA256 = SHA256.Create();
            long size = streamIn.Length;
            if (size == 0)
            {
                hash0 = null;
                status = "Empty file.";
                return false;
            }
            long rounds = size % chunkSize == 0 ? size / chunkSize + 1 : size / chunkSize; // modulo check
            int bytesRead,readSize;
            byte[] hashValue = null;
            byte[] buffer = new byte[chunkSize + hashSize];
            byte[] currentBuffer;
            hash0 = "";
            status = "Succesfull encoding";
            bool succeeded = true;

            for (long i = rounds; i >= 0; i--)
            {
                readSize = i == rounds ? (int)(size - rounds * chunkSize) : chunkSize;
                currentBuffer = i == rounds ? new byte[readSize] : buffer;
                try
                {
                    streamIn.Seek(i * chunkSize, SeekOrigin.Begin);
                    bytesRead = streamIn.Read(currentBuffer, 0, readSize);
                    if (hashValue != null)
                    {
                        Array.Copy(hashValue, 0, currentBuffer, chunkSize, hashValue.Length);
                    }
                    streamOut.Seek(i * (chunkSize + hashSize), SeekOrigin.Begin);
                    streamOut.Write(currentBuffer, 0, currentBuffer.Length);
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    status = ex.Message;
                    break;
                }
                hashValue = mySHA256.ComputeHash(currentBuffer);
            }
            if (succeeded)
            {
                hash0 = byteArrayToString(hashValue);
            }
            return succeeded;
        }

        // Decodes a file gets input and output file names and the first block hash
        // Creates the decoded file, returns a status message
        // Returns true if succecful
        public bool FileDecoder(string file_in_path, string file_out_path, string hash0,
                out string status)
        {
            FileStream streamIn = null;
            FileStream streamOut = null;
            try
            {
                streamIn = new FileStream(file_in_path, FileMode.Open, FileAccess.Read);
                streamOut = new FileStream(file_out_path, FileMode.Create);
                bool retValue = Decoder(streamIn, streamOut, hash0, out status);
                streamIn.Close();
                streamOut.Close();
                return retValue;
            }
            catch (Exception ex)
            {
                hash0 = "";
                status = ex.Message;
                if (streamIn != null)
                    streamIn.Close();
                return false;
            }
        }

        // Decodes a file gets input and output file streams and the first block hash
        // Creates the decoded file, returns a status message
        // Returns true if succecful
        private bool Decoder(FileStream streamIn,FileStream streamOut,
            string hash0, out string status)
        {
            SHA256 mySHA256 = SHA256.Create();
            long size = streamIn.Length;
            if (size == 0)
            {
                status = "Empty file.";
                return false;
            }
            long rounds = size / (chunkSize + hashSize);
            int writeSize = chunkSize;
            int readSize = chunkSize + hashSize;
            string lastHash = hash0;
            string currHash;
            byte[] readBuffer = new byte[hashSize + chunkSize];
            status = "Succesfull decoding";
            bool succeeded = true;

            for (long i = 0; i <= rounds; i++)
            {
                try
                {
                    streamIn.Seek(i * (chunkSize + hashSize), SeekOrigin.Begin);
                    streamOut.Seek(i * chunkSize, SeekOrigin.Begin);

                    if (i == rounds)
                    {
                        readSize = (int)(size % (chunkSize + hashSize));
                        writeSize = readSize;
                        readBuffer = new byte[readSize];
                    }
                    int bytesRead = streamIn.Read(readBuffer, 0,readSize);
                    currHash = byteArrayToString(mySHA256.ComputeHash(readBuffer)); 
                    if (currHash != lastHash)
                    {
                        status = "Wrong hash value";
                        succeeded = false;
                        break;
                    }
                    if (i != rounds)
                    {
                        lastHash = byteArrayToString(readBuffer, chunkSize);
                    }
                    streamOut.Write(readBuffer, 0, writeSize);
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    status = ex.Message;
                    break;
                }
            }
            streamIn.Close();
            return succeeded;
        }

        // Converts byte array to string
        private string byteArrayToString(byte[] ba, int startFrom = 0)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            for (int i = startFrom; i < ba.Length; i++)
                hex.AppendFormat("{0:x2}", ba[i]);
            return hex.ToString();
        }

        //Test function
        // For debug: returns the SHA256 of the entire file(cannot run on very large files)
        public string FullFileHash(string fileName)
        {
            byte[] fileInBytes = File.ReadAllBytes(fileName);
            SHA256 mySHA256 = SHA256.Create();
            byte[] hashValue = mySHA256.ComputeHash(fileInBytes);
            string s = byteArrayToString(hashValue);
            return s;
        }

    }
}
