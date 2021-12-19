using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace streamer_task
{
    class Tests
    {
        static void Main()
        {
            SecureFile e = new SecureFile();
            string path = GlobalData.TEST_DATA_PATH + "\\" ;
            string original_file = path + "zero.bin";
            string hash_protected_file = path + "zero_protected.bin";
            string reverse_original_file = path + "zero_reverse.bin";
            string hash0;
            string status;

            bool decodeSucceeded = e.FileEncoder(original_file, hash_protected_file, out hash0, out status);    
            string originalHash = e.FullFileHash(original_file);
            string protectedHash = e.FullFileHash(hash_protected_file);

            Console.WriteLine("hash0: " + hash0);
            Console.WriteLine("status: " + status);
            Console.WriteLine("originalHash: " + originalHash);
            Console.WriteLine("protectedHash: " + protectedHash);

            /*
            var t = Task.Run(() => e.FileDecoder(hash_protected_file, reverse_original_file, hash0, out status));
            Console.WriteLine("Before wait");
            t.Wait();
            */

            bool encodeSucceeded = e.FileDecoder(hash_protected_file, reverse_original_file, hash0, out status);
            Console.WriteLine("status: " + status);
            string reverseHash = e.FullFileHash(reverse_original_file);
            Console.WriteLine("reverseHash: " + reverseHash);
            if (reverseHash == originalHash)
            {
                Console.WriteLine("The original file is identical to the decoded file.");
            }
            else
            {
                Console.WriteLine("The original file is Not identical to the decoded file.");
            }
            Console.ReadLine();
        }
        
    }
}
