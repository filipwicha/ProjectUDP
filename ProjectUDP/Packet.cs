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
        public int numberToGues =0;//optional as correct range of numbers
        public int answer; //1 guesed /  0 not guesed
        public string sessionId = "";
        public string time { private set; get; } //variable with time
        public int length = 0;

        StringBuilder datagram = new StringBuilder(); //temporary string with data
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

        public Packet() { }

        //ACK packet
        public Packet(string sessionId)
        {
            this.sessionId = sessionId;
            this.answer = 2;
        }

        void Serialize()
        {
            collection.Add("O", leftNumber.ToString());
            collection.Add("o", answer.ToString());
            collection.Add("I", sessionId.ToString());
            collection.Add("RightNumber", rightNumber.ToString());
            collection.Add("NumberToGues", numberToGues.ToString());
            collection.Add("Time", DateTime.Now.ToString());

            foreach (var item in collection)
            {
                datagram.Append("#" + item.Key + "#$#" + item.Value + "#");
            }
        }

        public void Deserialize(byte[] received)
        {
            string message = Encoding.ASCII.GetString(received);
            Int32.TryParse(Regex.Match(message, @"(?<=#O#\$#)[0-9]+").Value, out leftNumber);
            Int32.TryParse(Regex.Match(message, @"(?<=#RightNumber#\$#)[0-9]+").Value, out rightNumber);
            Int32.TryParse(Regex.Match(message, @"(?<=#NumberToGues#\$#)[0-9]+").Value, out numberToGues);
            Int32.TryParse(Regex.Match(message, @"(?<=#o#\$#)[0-9]+").Value, out answer);
            sessionId = Regex.Match(message, @"(?<=#I#\$#)[a-zA-Z\-0-9]+").Value;
            time = Regex.Match(message, @"(?<=#Time#\$#)[0-9. :]+").Value;
        }
    }
}
