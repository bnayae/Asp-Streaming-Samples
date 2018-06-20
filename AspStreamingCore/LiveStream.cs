using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspStreamingCore
{
    public class LiveStream : Stream
    {
        private readonly IEnumerator<byte> _data;

        public LiveStream(IEnumerable<byte> data)
        {
            _data = data.GetEnumerator();
        }
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var chunck = GetChunck().ToArray();
            chunck.CopyTo(buffer, 0);
            return chunck.Length;

            IEnumerable<byte> GetChunck()
            {
                for (int i = 0; i < offset; i++)
                {
                    if (!_data.MoveNext())
                        yield break;
                }
                for (int i = 0; i < count; i++)
                {
                    if (!_data.MoveNext())
                        yield break;
                    yield return _data.Current;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
