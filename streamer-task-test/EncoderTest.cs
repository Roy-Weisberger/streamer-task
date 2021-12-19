using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamer_task;
using System.IO;
using System.Linq;

namespace streamer_task_test
{
    [TestClass]
    public class EncoderTest
    {
        private string path = GlobalData.TEST_DATA_PATH;

        [TestMethod]
        public void InFileNotFoundTest()
        {
            string fileName = "doesnt_exist.txt";
            string warning = "Could not find file '" + path + fileName + "'.";
            SecureFile e = new SecureFile();
            string original_file = path + fileName;
            string hash_protected_file = path + "resfile";
            string hash0;
            string status;
            bool encodeSucceeded = e.FileEncoder(original_file, hash_protected_file, out hash0, out status);
            Assert.AreEqual(warning, status, "Wrong file not found message.");
        }

        [TestMethod]
        public void OutPathNotFoundTest()
        {
            string warning = "Could not find a part of the path 'X:\\Temp\\Streamer\\resfile'.";
            SecureFile e = new SecureFile();
            string original_file = path + "zero.bin";
            string hash_protected_file = @"X:\\Temp\\Streamer\\resfile";
            string hash0;
            string status;
            bool encodeSucceeded = e.FileEncoder(original_file, hash_protected_file, out hash0, out status);
            Assert.AreEqual(warning, status, "Wrong path not found message.");
        }
        

        [TestMethod]
        public void EmptyFileTest()
        {
            string warning = "Empty file.";
            SecureFile e = new SecureFile();
            string original_file = path + "empty.txt";
            string hash_protected_file = path + "resfile";
            string hash0;
            string status;
            bool decodeSucceeded = e.FileEncoder(original_file, hash_protected_file, out hash0, out status);
            Assert.AreEqual(warning, status, "Wrong Input file is Empty message.");
        }
    }
}
