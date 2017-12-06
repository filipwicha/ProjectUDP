using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ProjectUDP
{
    class Packet
    {
        Dictionary<string, string> collection = new Dictionary<string, string>();

        public int leftNumber = 0; //or left lim of range
        public int rightNumber =0;//optional as correct range of numbers
        public int answer; //1 guesed /  0 not guesed
        public string sessionId = "";
        public string time { private set; get; }
        public int length = 0;

        StringBuilder datagram = new StringBuilder();
        public byte[] Bytes
        {
            get
            {
                Serialize();
                byte[] tmp = Encoding.ASCII.GetBytes(datagram.ToString());
                length = tmp.Length;
                return tmp;
            }
        }

        void Serialize()
        {
            collection.Add("LeftNumber", leftNumber.ToString());
            collection.Add("Answer", answer.ToString());
            collection.Add("SessionID", sessionId.ToString());
            collection.Add("RightNumber", rightNumber.ToString());
            collection.Add("Time", DateTime.Now.ToString());
            datagram.Append("#");

            foreach (var item in collection)
            {
                datagram.Append(item.Key + "#$#" + item.Value + "#");
            }
        }

        public void Deserialize(byte[] received)
        {
            string message = Encoding.ASCII.GetString(received);
            Int32.TryParse(Regex.Match(message, @"(?<=#LeftNumber#\$#)[0-9]+").Value, out leftNumber);
            Int32.TryParse(Regex.Match(message, @"(?<=#RightNumber#\$#)[0-9]+").Value, out rightNumber);
            Int32.TryParse(Regex.Match(message, @"(?<=#Answer#\$#)[0-9]+").Value, out answer);
            sessionId = Regex.Match(message, @"(?<=#SessionID#\$#)[a-zA-Z\-0-9]+").Value;
            time = Regex.Match(message, @"(?<=#Time#\$#)[0-9. :]+").Value;
        }
    }
}
