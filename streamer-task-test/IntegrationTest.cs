using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamer_task;
using System.IO;
using System.Linq;

namespace streamer_task_test
{
    [TestClass]
    public class IntegrationTest
    {
        private string path = GlobalData.TEST_DATA_PATH;

        [TestMethod]
        public void ZeroFileTest()
        {
            bool equalFiles = compareBeforeAfter(path + "zero.bin", path + "zero_reverse.bin");
            Assert.IsTrue(equalFiles, "Demo file after encoding and decoding are not the same (hash).");
        }


        [TestMethod]
        public void SmallFileTest()
        {
            bool equalFiles = compareBeforeAfter(path + "small_file.txt", path + "small_file_reverse.txt");
            Assert.IsTrue(equalFiles, "Small file after encoding and decoding are not the same (hash).");
        }

        [TestMethod]
        public void MediumSizeFileTest()
        {
            bool equalFiles = compareBeforeAfter(path + "medium_size_file.mp4", path + "medium_size_file_reverse.mp4");
            Assert.IsTrue(equalFiles, "Medium size file after encoding and decoding are not the same (hash).");
        }

        [TestMethod]
        public void DemoFileByDiffTest()
        {
            string original_file = path + "zero.bin";
            string hash_protected_file = path + "zero_protected_diff";
            bool sameFile = compareBeforeAfterWithDiff(original_file, hash_protected_file);
            Assert.IsTrue(sameFile, "File after encoding and decoding are not the same (diff).");

        }
        
        /*
        [TestMethod]
        public void TestLargeFile()
        {
            bool equalFiles = compareBeforeAfterWithDiff(path + "17.mp4", path + "17res");
            Assert.IsTrue(equalFiles, "Large file after encoding and decoding are not the same (diff).");
        }
        */
        
        private bool compareBeforeAfterWithDiff(string original_file, string reverse_file)
        {
            string protected_original_file = original_file + "_protected";

            SecureFile e = new SecureFile();
            string hash0;
            string status;
            bool decodeSucceeded = e.FileEncoder(original_file, protected_original_file, out hash0, out status);
            bool hla = e.FileDecoder(protected_original_file, reverse_file, hash0, out status);
            return FileEquals(original_file, reverse_file);
        }

        private bool compareBeforeAfter(string original_file,string reverse_file)
        {
            string protected_original_file = original_file+"_protected";

            SecureFile e = new SecureFile();
            string hash0;
            string status;
            bool decodeSucceeded = e.FileEncoder(original_file, protected_original_file, out hash0, out status);

            bool hla = e.FileDecoder(protected_original_file, reverse_file, hash0, out status);

            string originalFullHash = e.FullFileHash(original_file);
            string fileFromHashHash = e.FullFileHash(reverse_file);
            return originalFullHash == fileFromHashHash;
        }

        //function that compares 2 large files 
        
        static bool FileEquals(string fileName1, string fileName2)
        {
            using (var file1 = new FileStream(fileName1, FileMode.Open))
            using (var file2 = new FileStream(fileName2, FileMode.Open))
    
                return FileStreamEquals(file1, file2);
        }

        static bool FileStreamEquals(Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048;
            byte[] buffer1 = new byte[bufferSize]; //buffer size
            byte[] buffer2 = new byte[bufferSize];
            while (true)
            {
                int count1 = stream1.Read(buffer1, 0, bufferSize);
                int count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return false;

                if (count1 == 0)
                    return true;

                if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                    return false;
            }
        }
    }
}
