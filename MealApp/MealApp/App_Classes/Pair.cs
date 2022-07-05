using System;
using System.Xml.Serialization;
namespace MealApp
{

    [Serializable]
    public class Pair
    {
        [XmlAttribute]
        public string Name { get; set; }
        public object Value { get; set; }

        public Pair() { }


        public Pair(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public Pair(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Pair(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public Pair(string name, long value)
        {
            Name = name;
            Value = value;
        }

        public Pair(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public Pair(string name, DateTime value)
        {
            Name = name;
            Value = value;
        }

        public Pair(string name, bool value)
        {
            Name = name;
            Value = value;
        }
    }
}