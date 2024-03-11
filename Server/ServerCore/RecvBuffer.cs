using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    public class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos;
        int _writePos;

        public RecvBuffer(int size)
        {
            _buffer = new ArraySegment<byte>(new byte[size], 0, size);
            _readPos = 0;
            _writePos = 0;
        }

        public int DataSize { get { return _writePos - _readPos; } }

        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> DataSegment { get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); } }

        public ArraySegment<byte> FreeSegment { get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); } }

        public void Clear()
        {
            int dataSize = DataSize;
            if (dataSize == 0)
            {
                _readPos = 0;
                _writePos = 0;
            }
            else
            {
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        public bool OnRead(int numOfBytes)
        {
            if(numOfBytes > DataSize)
            {
                return false;
            }
            _readPos += numOfBytes;
            return true;
        }

        public bool OnWrite(int numOfBytes)
        {
            if(numOfBytes > FreeSize) 
            {
                return false;
            }
            _writePos += numOfBytes;
            return true;
        }
    }
}
