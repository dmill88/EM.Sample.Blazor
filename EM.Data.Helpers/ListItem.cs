using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Data.Helpers
{
    public class ListItem<TValue>
    {
        public TValue Value { get; set; } = default;
        public string Label { get; set; }
        public bool Selected { get; set; }
    }
}
