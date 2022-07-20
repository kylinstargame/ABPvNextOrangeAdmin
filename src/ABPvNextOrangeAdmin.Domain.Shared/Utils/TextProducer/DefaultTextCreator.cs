using System;
using System.Text;

namespace ABPvNextOrangeAdmin.Utils.TextProducer;

public class DefaultTextCreator : ITextProducer
{
    public DefaultTextCreator()
    {
    }

    public String getText()
    {
        int length = 8; //this.getConfig().getTextProducerCharLength();
        char[] chars = new char[] { }; //this.getConfig().getTextProducerCharString();
        Random rand = new Random();
        StringBuilder text = new StringBuilder();

        for (int i = 0; i < length; ++i)
        {
            text.Append(chars[rand.Next(chars.Length)]);
        }

        return text.ToString();
    }
}