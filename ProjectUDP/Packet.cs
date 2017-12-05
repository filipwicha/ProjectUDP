using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUDP
{
    public enum Answer
    {
        Guessed,
        notGuessed
    }

    class Packet
    {

        System.Collections.Generic.Dictionary<string, int> collection = new Dictionary<string, int>();

        int leftNumber; //or left lim of range
        Answer answer;
        int sessionId;
        int rightNumber;

        StringBuilder datagram;
        
        public Packet()
        {
            datagram = new StringBuilder();

        }

        public Packet(byte[] received)
        {
            datagram = append(Encoding.ASCII.GetString(received));
        }

        byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(datagram.ToString());
        }


        void Serialize()
        {
            collection.Add("LeftNumber", leftNumber);
            collection.Add("Answer", (int)answer);
            collection.Add("SessionID", sessionId);
            collection.Add("RightNumber", rightNumber);

            datagram.Append("#");

            foreach (var item in collection)
            {
                datagram.Append(item.Key + "#$#" + item.Value + "#");
            }
        }

        void Deserialize()
        {
            string message = "#LeftNumber#$#5#Answer#$#0#SesionID#$#4424412#RightNumber#$#5#";
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#LeftNumber#\$#)[0-9]+").Value, leftNumber);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#Answer#\$#)[0-9]+").Value, answer);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#SessionID#\$#)[0-9]+").Value, sessionId);
            Int32.TryParse(System.Text.RegularExpressions.Regex.Match(message, @"(?<=#RightNumber#\$#)[0-9]+").Value, rightNumber);
        }

    }
}
