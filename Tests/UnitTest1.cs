using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.BinaryExtensions;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var fs = new FileStream("l.bin", FileMode.OpenOrCreate))
            {
                var s = new S();
                s.hello = 5;
                s.world = "hgtr";
                s.g = new ST() { blub = 155693 };

                var bw = new BinaryWriter(fs);
                bw.Write(s);
            }

            using (var fs = new FileStream("l.bin", FileMode.OpenOrCreate))
            {
                var bw = new BinaryReader(fs);
                var s = bw.ReadStruct<S>();
            }
        }
        
        public struct S
        {
            public int hello;
            public string world;
            public ST g;
        }

        public class ST
        {
            public int blub;
        }
    }
}