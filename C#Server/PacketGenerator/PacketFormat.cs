using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    public class PacketFormat
    {
        //{0} : packet name
        //{1} : member attributes
        //{2} : member attribute read
        //{3} : member attribute write
        public static string packetFormat =
@"
public class {0}
{{
    {1}
    
    public void Read(ArraySegment<byte> segment)
    {{
        ushort count = 0;

        ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);

        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);

        bool success = true;
        ushort count = 0;

        Span<byte> span = new Span<byte>(openSegment.Array, openSegment.Offset, openSegment.Count);

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)PacketID.{0});
        count += sizeof(ushort);

        {3}
        
        success &= BitConverter.TryWriteBytes(span, count);

        if (success == false)
        {{
            return null;
        }}
        return SendBufferHelper.Close(count);
    }}
}}
";
        //{0} member type
        //{1} member name
        public static string memberForamt =
@"public {0} {1};";


        // {0} list name
        // {1} list instance name
        // {2} list member attributes
        // {3} list member read
        // {4} list member write
        public static string memberListFormat =
@"
public struct {0}
{{
    {2}

public void Read(ReadOnlySpan<byte>span, ref ushort count)
{{
    {3}
}}

public bool Write(Span<byte> span, ref ushort count)
{{
    bool success = true;
    {4}
    return success;
}}

    
}}
public List<{0}> {1}s = new List<{0}>();
";

        //{0} member type
        //{1} member name
        //{2} ToMemberType func
        public static string readFormat =
@"
this.{1} = BitConverter.{2}(span.Slice(count, span.Length - count));
count += sizeof({0});";

        //{0} member name
        public static string readStringFormat =
@"
ushort {0}Len = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(span.Slice(count, {0}Len));
count += {0}Len;
";
        // {0} list name
        // {1} list instance name
        public static string readListFormat =
@"
this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(span.Slice(count, span.Length-count));
count += sizeof(ushort);
for (int i = 0; i < {1}Len; i++)
{{
    {0} {1} = new {0}();
    {1}.Read(span, ref count);
    {1}s.Add({1});
}}
";
        //{0} member type
        //{1} member name
        public static string writeFormat =
@"
success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.{1});
count += sizeof({0});
";

        //{0} member name
        public static string writeStringFormat =
@"
ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, openSegment.Array, openSegment.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), {0}Len);
count += {0}Len;
count += sizeof(ushort);
";
        // {0} list name
        // {1} list instance name
        public static string writeListFormat =
@"
success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort){1}s.Count);
count += sizeof(ushort);
foreach({0} {1} in {1}s)
{{
    success &= {1}.Write(span, ref count);
}}
";
    }
}
