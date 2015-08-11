using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PokerLeagueManager.Common.Utilities
{
    public class XmlReaderFacade : IDisposable
    {
        private StringReader _reader;
        
        public XmlReaderFacade(string data)
        {
            _reader = new StringReader(data);

            try
            {
                this.XmlReader = XmlReader.Create(_reader);
            }
            catch
            {
                _reader.Dispose();
                throw;
            }
        }

        ~XmlReaderFacade()
        {
            Dispose(false);
        }

        public XmlReader XmlReader { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.XmlReader != null)
            {
                this.XmlReader.Dispose();
                _reader = null;
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }
        }
    }
}
