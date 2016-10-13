using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveTime.model
{
    public class Setting : IDBObjects 
    {
        private string settingID;

        private string mailAddress;
       
        private int numberTimecards;
       
        private bool isUsingTimelog;

    }
}
