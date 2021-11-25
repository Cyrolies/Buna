using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DALEFModel
{
    public class MultiSelectList 
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public int FilterByID { get; set; }

        List<MultiSelectItem> items = new List<MultiSelectItem>();
        public List<MultiSelectItem> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        public List<MultiSelectItem> GetCheckedItems
        {
            get
            {
                List<MultiSelectItem> checkedItems = new List<MultiSelectItem>();
                if (Items != null)
                {
                    foreach (MultiSelectItem item in Items)
                    {
                        if (item.Checked)
                        {
                            checkedItems.Add(item);
                        }
                    }
                }
                return checkedItems;
            }
        }

        
    }
    public class MultiSelectItem : SelectListItem
    {
        public bool Checked { get; set; }
    }
}
