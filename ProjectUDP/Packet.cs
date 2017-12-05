using System;
using System.Text;
using System.Collections.Generic;

namespace ProjectUDP
{
    class Packet
    {
        Dictionary<string, int> collection = new Dictionary<string, int>();

        public int leftNumber; //or left lim of range
        public int answer; //1 guesed /  0 not guesed
        public int sessionId;
        public int rightNumber;//optional as correct range of numbers

        StringBuilder datagram = new StringBuilder();
        public byte[] GetBytes()
        {
            Serialize();
            return Encoding.ASCII.GetBytes(datagram.ToString());
        }

        void Serialize()
        {
            collection.Add("LeftNumber", leftNumber);
            collection.Add("Answer", answer);
            collection.Add("SessionID", sessionId);
            collection.Add("RightNumber", rightNumber);

            datagram.Append("#");

            foreach (var item in collection)
            {
                datagram.Append(item.Key + "#$#" + item.Value + "#");
            }
        }

        public void Deserialize(byte[] received)
        {
            string message = Encoding.ASCII.GetString(received);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#LeftNumber#\$#)[0-9]+").Value, out leftNumber);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#Answer#\$#)[0-9]+").Value, out answer);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#SessionID#\$#)[0-9]+").Value, out sessionId);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#RightNumber#\$#)[0-9]+").Value, out rightNumber);
        }
    }
}
