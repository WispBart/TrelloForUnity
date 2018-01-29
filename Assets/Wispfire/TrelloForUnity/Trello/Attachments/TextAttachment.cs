using UnityEngine;

namespace Wispfire.TrelloForUnity
{
	    [SerializeField]
        public class TextAttachment : BinaryAttachment
        {
            string name;
            string content;
            string extension;
			
            public TextAttachment(string filename, string content, string extension = ".txt")
            {
                this.name = filename;
                this.content = content;
                this.extension = extension;
            }

            public string GetFilename()
            {
                return name + "." + extension;
            }
            public byte[] GetBytes()
            {
                return System.Text.Encoding.UTF8.GetBytes(content);
            }
        }
}
