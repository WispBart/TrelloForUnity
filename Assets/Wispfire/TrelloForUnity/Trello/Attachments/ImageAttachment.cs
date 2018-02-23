using UnityEngine;
using System;

namespace Wispfire.TrelloForUnity
{
    [Serializable]
    public class ImageAttachment : BinaryAttachment
    {
        public string name;
        public Texture2D image;
        public TextureEncoding encoding;

        public ImageAttachment(string name, Texture2D content, TextureEncoding encoding)
        {
            this.name = name;
            this.image = content;
            this.encoding = encoding;
        }

        public byte[] GetBytes()
        {
            byte[] bytes;
            switch (encoding)
            {
                case TextureEncoding.jpg:
                    bytes = image.EncodeToJPG();
                    break;
                case TextureEncoding.png:
                default:
                    bytes = image.EncodeToPNG();
                    break;
            }
            return bytes;
        }

        public string GetFilename()
        {
            return name + "." + encoding.ToString();
        }

        public void OnDestroy()
        {
            Texture2D.Destroy(image);
        }

        public enum TextureEncoding { jpg, png }
    }
}
