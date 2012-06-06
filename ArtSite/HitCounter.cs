using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ArtSite
{
    public class HitCounter
    {
        private static string _path;
       // private object _lockObject = null;

        public static long GetHits()
        {            
            long val = 0;
            if (File.Exists(_path))
            {
                using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new BinaryReader(stream))
                {
                    val = reader.ReadInt64();
                }
            }

            return val;
        }

        public static void IncrementHitCounter()
        {
            _path = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath).Replace("/", "\\") +"\\hits.bin";

            long val = GetHits();
            using (var stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(++val);
            }
        }
    }
}