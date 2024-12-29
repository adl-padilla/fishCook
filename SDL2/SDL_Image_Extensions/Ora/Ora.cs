using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace SDL2.SDL_Image_Extensions.Ora
{
    [XmlRoot(ElementName = "layer")]
    public class Layer
    {

        [XmlAttribute(AttributeName = "composite-op")]
        public string CompositeOp { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public int Name { get; set; }

        [XmlAttribute(AttributeName = "opacity")]
        public double Opacity { get; set; }

        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }

        [XmlAttribute(AttributeName = "visibility")]
        public string Visibility { get; set; }

        [XmlAttribute(AttributeName = "x")]
        public int X { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public int Y { get; set; }
    }

    [XmlRoot(ElementName = "stack")]
    public class Stack
    {

        [XmlElement(ElementName = "layer")]
        public List<Layer> Layer { get; set; }
    }

    [XmlRoot(ElementName = "image")]
    public class Image
    {

        [XmlElement(ElementName = "stack")]
        public Stack Stack { get; set; }

        [XmlAttribute(AttributeName = "h")]
        public int H { get; set; }

        [XmlAttribute(AttributeName = "w")]
        public int W { get; set; }
    }
}
