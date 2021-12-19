using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamer_task;
using System.IO;
using System.Linq;

namespace streamer_task_test
{
    [TestClass]
    public class DecoderTest
    {
        private string path = GlobalData.TEST_DATA_PATH;
        
        [TestMethod]
        public void InFileNotFoundTest()
        {
            string fileName = "doesnt_exist.txt";
            string warning = "Could not find file '" +path + fileName + "'.";
            SecureFile e = new SecureFile();
            string original_file = path + fileName;
            string hash_protected_file = path + "protected_empty1111";
            string hash0 = "95b532cc4381affdff0d956e12520a04129ed49d37e154228368fe5621f0b9a3";
            string status;
            bool decodeSucceeded = e.FileDecoder(original_file, hash_protected_file,hash0, out status);
            Assert.AreEqual(warning, status,"Wrong file not found message.");
        }

        [TestMethod]
        public void IncorrectHash0Test()
        {
            string warning = "Wrong hash value";
            SecureFile e = new SecureFile();
            string original_file = path + "zero.bin";
            string hash_protected_file = path + "protected_zero";
            string hash0 = "95b532cc4381affdff0d956e12520a04129ed49d37e154228368fe5621f0b9a3";
            string status;
            bool decodeSucceeded = e.FileDecoder(original_file, hash_protected_file, hash0, out status);
            Assert.AreEqual(warning, status, "Wrong hash value message.");
        }

        [TestMethod]
        public void EmptyFileTest()
        {
            string warning = "Empty file.";
            SecureFile e = new SecureFile();
            string original_file = path + "empty.txt";
            string hash_protected_file = path + "protected_empty";
            string hash0 = "95b532cc4381affdff0d956e12520a04129ed49d37e154228368fe5621f0b9a3";
            string status;
            bool decodeSucceeded = e.FileDecoder(original_file, hash_protected_file, hash0, out status);
            Assert.AreEqual(warning, status, "Wrong empty file message.");
        }
    }
}
