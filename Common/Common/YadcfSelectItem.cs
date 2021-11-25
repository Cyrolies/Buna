using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class YadcfSelectItem
    {
        private string value;
        private string label;

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }

            set
            {
                label = value;
            }
        }
    }
}
