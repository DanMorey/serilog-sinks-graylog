using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Formatting.Compact.Reader;
internal class TextException : Exception
{
    readonly string _text;

    public TextException(string text)
        : base("This exception type provides ToString() access to details only.")
    {
        _text = text;
    }

    public override string ToString()
    {
        return _text;
    }
}
