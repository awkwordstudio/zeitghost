using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SysInfo
{
    public partial class Main_Form
    {
        private string _returnURLStirng()
        {
            string qury = @"case when Substring(url, 1, Charindex('.', url) - 1)= 'www' then 
                                Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url) + 1, LEN(url))) - 1) 
                                else Substring(url, 1, Charindex('.', url) - 1) end";
            return qury;
        }

        private string _returnTimeString(Int32 tm,string alias)
        {
            return "CONVERT(varchar(10), ISNULL(CAST(sum(cast(datediff(second, "+alias+".start_time, "+alias+".end_time) as float) / " + tm.ToString() + ") AS numeric(10, 2)),0.0)) ";
            //return "ISNULL(round(sum(DateDiff(SECOND,start_time,end_time))/"+ tm.ToString()+ ",1),0.0)";
        }
    }
}
